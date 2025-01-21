using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public enum EGameMode //перечисление уровней
    {
        NOT_SET,
        EASY,
        MEDIUM,
        HARD, 
        VERY_HARD
    }

    public static GameSettings Instance;

    private void Awake()
    {
        Paused = false;
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else Destroy(this);
    }

    private EGameMode GameMode;
    private bool Paused = false;
    private bool continueGame = false;
    private bool exit = false;

    public void SetExitAfterWin(bool set)
    {
        exit = set;
        continueGame = false;
    }

    public bool GetExitAfterWin()
    {
        return exit;
    }

    public void SetContinueGame(bool continue_game)
    {
        continueGame = continue_game;
    }
    public bool GetContinueGame()
    {
        return continueGame;
    }
    public void SetPaused(bool paused) { Paused = paused; }
    public bool GetPaused() { return Paused; }

    void Start()
    {
        GameMode = EGameMode.NOT_SET;
        continueGame = false;
    }

    public void SetGameMode(EGameMode mode) 
    {
        GameMode = mode;
    }

    public void SetGameMode(string mode)
    {//установление игрового режима
        if (mode == "Easy") SetGameMode(EGameMode.EASY);
        else if (mode == "Medium") SetGameMode(EGameMode.MEDIUM);
        else if (mode == "Hard") SetGameMode(EGameMode.HARD);
        else if (mode == "VeryHard") SetGameMode(EGameMode.VERY_HARD);
        else SetGameMode(EGameMode.NOT_SET);
    }

    public string GetGameMode() //для получения строкой режима игры
    {
        switch (GameMode)
        {
            case EGameMode.EASY: return "Easy";
            case EGameMode.MEDIUM: return "Medium";
            case EGameMode.HARD: return "Hard";
            case EGameMode.VERY_HARD: return "VeryHard";
        }
        Debug.LogError("Ошибка! Уровень не установлен.");
        return " ";
    }
}
