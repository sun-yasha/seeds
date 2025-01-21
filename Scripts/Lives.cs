using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> error_images; //список игровых объектов-картинок для ошибок
    public GameObject game_over_popup;
    int lives_ = 0; //жизни
    int error_number = 0;
    public static Lives instance;

    private void Awake()
    {
        if(instance) Destroy(instance);
        instance = this;
    }
    void Start()
    {
        lives_ = error_images.Count; //количество жизней = количеству изображений-ошибок
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
        {//если число ошибок меньше числа изображений, то активируем изображения-ошибки
            error_images[error_number].SetActive(true); 
            error_number++; //число ошибок увеличивается
            lives_--; //жизни уменьшаются
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
