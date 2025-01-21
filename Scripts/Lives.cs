using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> error_images; //������ ������� ��������-�������� ��� ������
    public GameObject game_over_popup;
    int lives_ = 0; //�����
    int error_number = 0;
    public static Lives instance;

    private void Awake()
    {
        if(instance) Destroy(instance);
        instance = this;
    }
    void Start()
    {
        lives_ = error_images.Count; //���������� ������ = ���������� �����������-������
        error_number = 0;

        if(GameSettings.Instance.GetContinueGame())
        {
            error_number = Config.ErrorNumber();
            lives_ = error_images.Count - error_number;
            for(int error = 0; error < error_number; error++)
            {
                error_images[error].SetActive(true);
            }
        }
    }

    public int GetErrorNumber() { return error_number; }

    private void WrongNumber()
    {
        if (error_number < error_images.Count)
        {//���� ����� ������ ������ ����� �����������, �� ���������� �����������-������
            error_images[error_number].SetActive(true); 
            error_number++; //����� ������ �������������
            lives_--; //����� �����������
        }
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if(lives_ <= 0)
        {
            GameEvents.OnGameOverMethod();
            game_over_popup.SetActive(true);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }
}
