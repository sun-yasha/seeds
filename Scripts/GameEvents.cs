using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static GameEvents;

public class GameEvents : MonoBehaviour
{
    public delegate void CheckBoardCompleted();
    public static event CheckBoardCompleted OnCheckBoardCompleted;
    public static void CheckBoardCompletedMethod()
    {
        if(OnCheckBoardCompleted != null) OnCheckBoardCompleted();
    }

    public delegate void UpdateSquareNumber(int number);
    public static event UpdateSquareNumber OnUpdateSquareNumber; //событие обновления данных квадратов сетки
    public static void UpdateSquareNumberMethod(int number)
    {//каждая функция, назначенная событию, будет выполнена при вызове
        if (OnUpdateSquareNumber != null) OnUpdateSquareNumber(number);
    }


    public delegate void SquareSelected(int square_index);
    public static event SquareSelected OnSquareSelected; //событие выбора ячейки сетки

    public static void SquareSelectedMethod(int square_index) //функция вызывается тогда, когда мы выбираем какой-либо квадрат сетки
    {
        if (OnSquareSelected != null) OnSquareSelected(square_index); //назначение индекса
    }

    public delegate void WrongNumber();
    public static event WrongNumber OnWrongNumber; //событие выбора неправильного значения квадрата

    public static void OnWrongNumberMethod()
    {//если число ошибок не равно 0, то вызываем метод неправильного выбора
        if (OnWrongNumber != null) OnWrongNumber();
    }
    public delegate void GameOver();
    public static event GameOver OnGameOver;
    public static void OnGameOverMethod()
    {
        if (OnGameOver != null) OnGameOver();
    }


    public delegate void NotesActive(bool active);
    public static event NotesActive OnNotesActive;
    public static void OnNotesActiveMethod(bool active) //метод активации режима заметок
    {
        if (OnNotesActive != null) OnNotesActive(active);
    }


    public delegate void ClearNumber();
    public static event ClearNumber OnClearNumber;
    public static void OnClearNumberMethod() //метод активации режима ластика
    {
        if (OnClearNumber != null) OnClearNumber();
    }


    public delegate void BoardCompleted();
    public static event BoardCompleted OnBoardCompleted;
    public static void OnBoardCompletedMethod() 
    {
        if (OnBoardCompleted != null) OnBoardCompleted();
    }
}
