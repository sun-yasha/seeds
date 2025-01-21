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
    private int number_ = 0; //число хранящееся в квадрате сетки
    public List<GameObject> number_notes; //список игровых объектов, в которые записываются числа-заметки
    private bool note_active; 
    private int correct_number = 0; //правильное значение квадрата
    private bool selected_ = false; //переменная выбора
    private int square_index_ = -1; //индекс квадрата сетки
    private bool has_default_value = false; //значение в клетках сетки по умолчанию
    private bool has_wrong_value = false;

    public int GetSquareNumber() { return number_; }
    public bool IsCorrectNumberSet() { return number_ == correct_number; }
    public bool HasWrongValue() { return has_wrong_value; }
    public void SetHasDefaultValue(bool has_default) { has_default_value = has_default; }
    public bool GetHasDefaultValue() { return has_default_value; }
    public bool IsSelected() { return selected_; } //возвращает выбранное значение

    public void SetSquareIndex(int index)
    {//устанавливает индекс
        square_index_ = index;
    }

    public void SetCorrectNumber(int number)
    {//правильное число равно передаваемому числу
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
        if (GameSettings.Instance.GetContinueGame() == false) SetNoteNumber(0); //все строки объекта заметок пустые
        else SetClearEmptyNotes();
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    //ЗАМЕТКИ

    public List<string> GetSquareNotes()
    {
        List<string> notes = new List<string>(); //список заметок
        foreach (var number in number_notes)
        {
            notes.Add(number.GetComponent<Text>().text); //добавление текста в игровой объект
        }
        return notes;
    }

    private void SetClearEmptyNotes() //набор пустых значений 
    {
        foreach (var number in number_notes)
        { //Если значение текста в игровом объекте = 0, то она отображает пустую строку
            if (number.GetComponent<Text>().text == "0") number.GetComponent<Text>().text = " ";
        }
    }

    private void SetNoteNumber(int value) //числовое значение заметок
    {
        foreach(var number in number_notes)
        {//Если значение в тексте объекта = 0, то в это поле записывается выбранное нами число
            if (value <= 0) number.GetComponent<Text>().text = " ";
            else number.GetComponent<Text>().text = number_.ToString();
        }
    }

    private void SetNoteSingleNumber(int value, bool force_update = false)
    {//Если игровой объект неактивен, то оставляем без изменения
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

    public void OnNotesActive(bool active)//устанавливает значение того, что передадим
    {
        note_active = active;
    }
    //---------------------------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------

    public void DisplayText() //запись отображаемого текста
    {//если значение равно нулю, то клетка пустая
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
        selected_ = true; //при щелчке - ячейка выбрана
        GameEvents.SquareSelectedMethod(square_index_); //метод выбора квадрата
    }

    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable() //функция при включении объекта
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber; //событие обновление данных
        GameEvents.OnSquareSelected += OnSquareSelected; //событие выбора квадрата сетки
        GameEvents.OnNotesActive += OnNotesActive; //событие активности заметок
        GameEvents.OnClearNumber += OnClearNumber; //событие активности ластика
    }

    private void OnDisable() //функция при отключении объекта
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnNotesActive -= OnNotesActive;
        GameEvents.OnClearNumber -= OnClearNumber;
    }

    public void OnClearNumber()
    {//если выбран квадрат сетки и у него нет значения по умолчанию
        if(selected_ && !has_default_value)
        {
            number_ = 0; //стирает числа в клетках
            has_wrong_value = false;
            SetNoteNumber(0); //стирает заметки
            DisplayText();
        }
        SetSquareColor(Color.white); //меняет цвет клетки
    }

    public void OnSetNumber(int number)
    {//если выбран квадрат сетки и у него нет значения по умолчанию
        if (selected_ && has_default_value == false)
        {
            if (note_active == true && has_wrong_value == false)//режим заметок активен, 
            {
                SetNoteSingleNumber(number);
            }
            else if (note_active == false)
            {
                SetNoteNumber(0); //все строки объекта заметок пустые
                SetNumber(number);

                if (number_ != correct_number)
                {//если введёное число не равно правильному, то квадрат выделяем цветом
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
    {//если изначальный индекс не равен передаваемому, то квадрат не выбран
        if (square_index_ != square_index) selected_ = false;
    }

    public void SetSquareColor(Color col)
    {
        var colors = this.colors;
        colors.normalColor = col;
        this.colors = colors;
    }
}
