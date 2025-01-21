using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(string name) //загрузка сцен
    {
        SceneManager.LoadScene(name);
    }

    //загрузка сцен с режимами игры
    public void LoadEasyGame(string name)
    {//установление режима
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.EASY);
        SceneManager.LoadScene(name);
    }

    public void LoadMediumGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.MEDIUM);
        SceneManager.LoadScene(name);
    }

    public void LoadHardGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.HARD);
        SceneManager.LoadScene(name);
    }

    public void LoadVeryHardGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.VERY_HARD);
        SceneManager.LoadScene(name);
    }

    public void ActivateObject(GameObject obj) //активация игровых объектов
    {
        obj.SetActive(true);
    }

    public void DeActivateObject(GameObject obj) //деактивация игровых объектов
    {
        obj.SetActive(false);
    }

    public void SetPause(bool paused)
    {
        GameSettings.Instance.SetPaused(paused);
    }


    public void ContinueGame(bool continue_game)
    {
        GameSettings.Instance.SetContinueGame(continue_game);
    }

    public void ExitWin()
    {
        GameSettings.Instance.SetExitAfterWin(true);
    }
}
