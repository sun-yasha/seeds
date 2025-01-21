using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class SudokuGrid : MonoBehaviour
{
    public int columns = 0;
    public int rows = 0;
    public float square_offset = 0.0f; //смещение
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0.0f, 0.0f); //начальная позиция сетки
    public float square_scale = 1.0f;
    private List<GameObject> grid_squares = new List<GameObject>(); //список квадратов сетки
    private int selected_grid_data = -1;
    public float square_gap = 0.1f;
    public Color line_highlight_color = Color.white;

    void Start()
    {
        if (grid_square.GetComponent<GridSquare>() == null) Debug.LogError("This Game Object need to have GridSquare script attached!");
        CreateGrid();
        SetGridNumber(GameSettings.Instance.GetGameMode()); //доступ к настройкам игры для активации выбранного режима игры

        if (GameSettings.Instance.GetContinueGame()) SetGridFormFile();
        else SetGridNumber(GameSettings.Instance.GetGameMode());
    }

    void SetGridFormFile()
    {
        string level = GameSettings.Instance.GetGameMode();
        selected_grid_data = Config.ReadGameBoardLevel();
        var data = Config.ReadGridData();
        setGridSquareData(data);
        SetGridNotes(Config.GetGridNotes());
    }

    private void SetGridNotes(Dictionary<int, List<int>> notes)
    {
        foreach(var note in notes)
        {
            grid_squares[note.Key].GetComponent<GridSquare>().SetGridNotes(note.Value);
        }
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    { 
        //0, 1, 2, 3...
        //7, 8
        int square_index = 0;
        for(int row = 0; row < rows; row++) //строки
        {
            for(int column = 0; column < columns; column++) //столбцы
            {
                grid_squares.Add(Instantiate(grid_square) as GameObject); //установление списка квадратов сетки как игрового объекта
                grid_squares[grid_squares.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index); //установка индекса квадрата сетки
                grid_squares[grid_squares.Count - 1].transform.parent = this.transform;
                grid_squares[grid_squares.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale); //сетка квадратов с указанными размерами
                square_index++;
            }
        }
    }

    private void SetSquaresPosition()
    {
        var square_rect = grid_squares[0].GetComponent<RectTransform>(); //создание общего квадрата сетки
        Vector2 offset = new Vector2(); //промежуток между квадратами
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + square_offset; //смещение квадратов по ширине
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + square_offset; //смещение квадратов по высоте

        int column_number = 0;
        int row_number = 0;

        foreach(GameObject square in grid_squares)
        {
            if(column_number + 1 > columns)//создание квадратов не больше установленного количества
            {
                row_number++; //номер строки
                column_number = 0; //сбрасываем номер столбца, чтобы начать с нуля
                square_gap_number.x = 0;
                row_moved = false;
            }
            var pos_x_offset = offset.x * column_number + (square_gap_number.x * square_gap); //рассчёт смещений
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * square_gap);
            if(column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }
            if(row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += square_gap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(start_position.x + pos_x_offset, start_position.y - pos_y_offset); //установление положения сетки
            column_number++;
        }
    }

    private void SetGridNumber(string level) //номера клеток сетки
    {
        selected_grid_data = Random.Range(0, SudokuData.Instance.sudoku_game[level].Count); //данные сетки записываются в диапазон точек от 0 до соответствующих уровню данных
        var data = SudokuData.Instance.sudoku_game[level][selected_grid_data];
        setGridSquareData(data);
        //foreach(var square in grid_squares)
        //{
        //    square.GetComponent<GridSquare>().SetNumber(Random.Range(0, 10));
        //}
    }

    private void setGridSquareData(SudokuData.SudokuBoardData data) //принимает данные для доски
    {//цикл по всем квадратам сетки
        for (int index = 0; index < grid_squares.Count; index++)
        {//получаем индексы квадратов сетки и устанавливаем значения нерешённых и решённых данных 
            grid_squares[index].GetComponent<GridSquare>().SetNumber(data.unsolved_data[index]); 
            grid_squares[index].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[index]); //правильные значения квадратов сетки из данных сетки
            grid_squares[index].GetComponent<GridSquare>().SetHasDefaultValue(data.unsolved_data[index] != 0 && data.unsolved_data[index] == data.solved_data[index]);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnCheckBoardCompleted += CheckBoardCompleted;
    }
    private void OnDisable()
    {
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnCheckBoardCompleted -= CheckBoardCompleted;

        var solved_data = SudokuData.Instance.sudoku_game[GameSettings.Instance.GetGameMode()][selected_grid_data].solved_data;
        int[] unsolved_data = new int[81];
        Dictionary<string, List<string>> grid_notes = new Dictionary<string, List<string>>();
        for (int i = 0; i < grid_squares.Count; i++)
        {
            var comp = grid_squares[i].GetComponent<GridSquare>();
            unsolved_data[i] = comp.GetSquareNumber();
            string key = "square_note:" + i.ToString();
            grid_notes.Add(key, comp.GetSquareNotes());
        }
        SudokuData.SudokuBoardData current_game_data = new SudokuData.SudokuBoardData(unsolved_data, solved_data);
        if (GameSettings.Instance.GetExitAfterWin() == false)
            Config.SaveBoardData(current_game_data, GameSettings.Instance.GetGameMode(), selected_grid_data, Lives.instance.GetErrorNumber(), grid_notes);
        else Config.DeleteDataFile();

        GameSettings.Instance.SetExitAfterWin(false);
    }

    private void SetSquaresColor(int[] data, Color col)
    {
        foreach(var index in data)
        {
            var comp = grid_squares[index].GetComponent<GridSquare>();
            if(comp.HasWrongValue() == false &&  comp.IsSelected() == false)
            {
                comp.SetSquareColor(col);
            }
        }
    }

    public void OnSquareSelected(int square_index)
    {
        var horizontal_line = IndicatorLine.instance.GetHorizontalLine(square_index);
        var vertical_line = IndicatorLine.instance.GetVerticalLine(square_index);
        var square = IndicatorLine.instance.GetSquare(square_index);
        SetSquaresColor(IndicatorLine.instance.GetAllSquaresIndex(), Color.white);
        SetSquaresColor(horizontal_line, line_highlight_color);
        SetSquaresColor(vertical_line, line_highlight_color);
        SetSquaresColor(square, line_highlight_color);
    }


    private void CheckBoardCompleted()
    {
        foreach(var square in grid_squares)
        {
            var comp = square.GetComponent<GridSquare>();
            if (comp.IsCorrectNumberSet() == false) return;
        }
        GameEvents.OnBoardCompletedMethod();
    }

    public void SolveSudoku()
    {
        foreach(var square in grid_squares)
        {
            var comp = square.GetComponent<GridSquare>();
            comp.SetCorrectNumber2();
        }
        CheckBoardCompleted();
    }
}
