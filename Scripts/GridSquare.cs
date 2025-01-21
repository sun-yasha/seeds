using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GameEvents;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject number_text;
    private int number_ = 0; //����� ���������� � �������� �����
    public List<GameObject> number_notes; //������ ������� ��������, � ������� ������������ �����-�������
    private bool note_active; 
    private int correct_number = 0; //���������� �������� ��������
    private bool selected_ = false; //���������� ������
    private int square_index_ = -1; //������ �������� �����
    private bool has_default_value = false; //�������� � ������� ����� �� ���������
    private bool has_wrong_value = false;

    public int GetSquareNumber() { return number_; }
    public bool IsCorrectNumberSet() { return number_ == correct_number; }
    public bool HasWrongValue() { return has_wrong_value; }
    public void SetHasDefaultValue(bool has_default) { has_default_value = has_default; }
    public bool GetHasDefaultValue() { return has_default_value; }
    public bool IsSelected() { return selected_; } //���������� ��������� ��������

    public void SetSquareIndex(int index)
    {//������������� ������
        square_index_ = index;
    }

    public void SetCorrectNumber(int number)
    {//���������� ����� ����� ������������� �����
        correct_number = number;
        has_wrong_value = false;

        if (number_ != 0 && number_ != correct_number)
        {
            has_wrong_value = true;
            SetSquareColor(Color.red);
        }
    }

    public void SetCorrectNumber2()
    {
        number_ = correct_number;
        SetNoteNumber(0);
        DisplayText();
    }

    void Start()
    {
        selected_ = false;
        note_active = false;
        if (GameSettings.Instance.GetContinueGame() == false) SetNoteNumber(0); //��� ������ ������� ������� ������
        else SetClearEmptyNotes();
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    //�������

    public List<string> GetSquareNotes()
    {
        List<string> notes = new List<string>(); //������ �������
        foreach (var number in number_notes)
        {
            notes.Add(number.GetComponent<Text>().text); //���������� ������ � ������� ������
        }
        return notes;
    }

    private void SetClearEmptyNotes() //����� ������ �������� 
    {
        foreach (var number in number_notes)
        { //���� �������� ������ � ������� ������� = 0, �� ��� ���������� ������ ������
            if (number.GetComponent<Text>().text == "0") number.GetComponent<Text>().text = " ";
        }
    }

    private void SetNoteNumber(int value) //�������� �������� �������
    {
        foreach(var number in number_notes)
        {//���� �������� � ������ ������� = 0, �� � ��� ���� ������������ ��������� ���� �����
            if (value <= 0) number.GetComponent<Text>().text = " ";
            else number.GetComponent<Text>().text = number_.ToString();
        }
    }

    private void SetNoteSingleNumber(int value, bool force_update = false)
    {//���� ������� ������ ���������, �� ��������� ��� ���������
        if (note_active == false && force_update == false) return;

        if (value <= 0) number_notes[value - 1].GetComponent<Text>().text = " ";
        else
        {
            if (number_notes[value - 1].GetComponent<Text>().text == " " || force_update) number_notes[value - 1].GetComponent<Text>().text = value.ToString();
            else number_notes[value - 1].GetComponent<Text>().text = " ";
        }
    }

    public void SetGridNotes(List<int> notes)
    {
        foreach(var note in notes)
        {
            SetNoteSingleNumber(note, true);
        }
    }

    public void OnNotesActive(bool active)//������������� �������� ����, ��� ���������
    {
        note_active = active;
    }
    //---------------------------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------

    public void DisplayText() //������ ������������� ������
    {//���� �������� ����� ����, �� ������ ������
        if (number_ <= 0) number_text.GetComponent<Text>().text = " ";
        else number_text.GetComponent<Text>().text = number_.ToString();
    }

    public void SetNumber(int number)
    {
        number_ = number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true; //��� ������ - ������ �������
        GameEvents.SquareSelectedMethod(square_index_); //����� ������ ��������
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable() //������� ��� ��������� �������
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber; //������� ���������� ������
        GameEvents.OnSquareSelected += OnSquareSelected; //������� ������ �������� �����
        GameEvents.OnNotesActive += OnNotesActive; //������� ���������� �������
        GameEvents.OnClearNumber += OnClearNumber; //������� ���������� �������
    }

    private void OnDisable() //������� ��� ���������� �������
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
        GameEvents.OnClearNumber -= OnClearNumber;
    }

    public void OnClearNumber()
    {//���� ������ ������� ����� � � ���� ��� �������� �� ���������
        if(selected_ && !has_default_value)
        {
            number_ = 0; //������� ����� � �������
            has_wrong_value = false;
            SetNoteNumber(0); //������� �������
            DisplayText();
        }
        SetSquareColor(Color.white); //������ ���� ������
    }

    public void OnSetNumber(int number)
    {//���� ������ ������� ����� � � ���� ��� �������� �� ���������
        if (selected_ && has_default_value == false)
        {
            if (note_active == true && has_wrong_value == false)//����� ������� �������, 
            {
                SetNoteSingleNumber(number);
            }
            else if (note_active == false)
            {
                SetNoteNumber(0); //��� ������ ������� ������� ������
                SetNumber(number);

                if (number_ != correct_number)
                {//���� ������� ����� �� ����� �����������, �� ������� �������� ������
                    has_wrong_value = true;
                    var colors = this.colors;
                    colors.normalColor = Color.red;
                    this.colors = colors;

                    GameEvents.OnWrongNumberMethod();
                }
                else
                {
                    has_wrong_value = false;
                    has_default_value = true;
                    var colors = this.colors;
                    colors.normalColor = Color.white;
                    this.colors = colors;
                }
            }
            GameEvents.CheckBoardCompletedMethod();
        }
    }

    public void OnSquareSelected(int square_index)
    {//���� ����������� ������ �� ����� �������������, �� ������� �� ������
        if (square_index_ != square_index) selected_ = false;
    }

    public void SetSquareColor(Color col)
    {
        var colors = this.colors;
        colors.normalColor = col;
        this.colors = colors;
    }
}
