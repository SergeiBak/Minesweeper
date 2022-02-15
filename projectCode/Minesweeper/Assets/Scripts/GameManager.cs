using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int width = 16;
    [SerializeField]
    private int height = 16;
    [SerializeField]
    private int mineCount = 32;

    [SerializeField]
    private float borderThickness = 0.5f;
    [SerializeField]
    private GameObject background;

    private Board board;
    private Cell[,] state;
    private bool gameOver;

    private bool firstClick = true;

    private void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width * height); // makes sure we don't accidentally choose more mines than slots on board
    }

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }

        if (!gameOver)
        {
            if (Input.GetMouseButtonDown(1)) // flagging
            {
                Flag();
            }
            else if (Input.GetMouseButtonDown(0)) // revealing
            {
                if (firstClick)
                {
                    firstClick = false;

                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // mouse position converted to world space position
                    Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition); // world space position to tilemap/cell position
                    Cell cell = GetCell(cellPosition.x, cellPosition.y);

                    GenerateMines(cell);
                    GenerateNumbers();
                }

                Reveal();
            }
            else if (Input.GetMouseButtonDown(2)) // reveal all tiles around number tile if equal number of mines are flagged
            {
                AutoFillSurroundingCells();
            }
        }
    }

    private void AutoFillSurroundingCells()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // mouse position converted to world space position
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition); // world space position to tilemap/cell position
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type != Cell.Type.Number || !cell.revealed) // cell must be a revealed number to check outer cells
        {
            return;
        }

        CheckSurroundingCells(cell);
    }

    private void CheckSurroundingCells(Cell cell)
    {
        if (!AnyCellsUnrevealed(cell.position.x, cell.position.y)) // if all surrounding cells are already revealed, no point in checking and flipping
        {
            return;
        }

        if (CountMines(cell.position.x, cell.position.y) != CountFlags(cell.position.x, cell.position.y)) // slots flagged not equal to number of hidden mines
        {
            return;
        }

        RevealSurroundingCells(cell.position.x, cell.position.y);
    }

    private void RevealSurroundingCells(int cellX, int cellY)
    {
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) // current cell being checked, we skip
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                Cell currentCell = GetCell(x, y);


                if (currentCell.type == Cell.Type.Invalid || currentCell.revealed || currentCell.flagged)
                {
                    continue;
                }

                switch (currentCell.type)
                {
                    case Cell.Type.Mine: // if bomb tile, explode
                        Explode(currentCell);
                        break;
                    case Cell.Type.Empty: // if empty tile, start flooding
                        Flood(currentCell);
                        CheckWinCondition();
                        break;
                    default:
                        currentCell.revealed = true;
                        state[x, y] = currentCell;
                        CheckWinCondition();
                        break;
                }

                board.Draw(state);
            }
        }
    }

    private void Flag() // places flag on cell if it is valid and unrevealed
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // mouse position converted to world space position
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition); // world space position to tilemap/cell position
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed)
        {
            return;
        }

        cell.flagged = !cell.flagged;
        state[cellPosition.x, cellPosition.y] = cell;
        board.Draw(state);
    }

    private void Reveal()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // mouse position converted to world space position
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition); // world space position to tilemap/cell position
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
        {
            return;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine: // if bomb tile, explode
                Explode(cell);
                break;
            case Cell.Type.Empty: // if empty tile, start flooding
                Flood(cell);
                CheckWinCondition();
                break;
            default:
                cell.revealed = true;
                state[cellPosition.x, cellPosition.y] = cell;
                CheckWinCondition();
                break;
        }

        board.Draw(state);
    }

    private void Flood(Cell cell) // goes through and reveals all connected empty tiles
    {
        if (cell.revealed || cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid)
        {
            return;
        }

        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Empty) // continue flooding if cell empty
        {
            Flood(GetCell(cell.position.x - 1, cell.position.y)); // left
            Flood(GetCell(cell.position.x + 1, cell.position.y)); // right
            Flood(GetCell(cell.position.x, cell.position.y - 1)); // down
            Flood(GetCell(cell.position.x, cell.position.y + 1)); // up

            Flood(GetCell(cell.position.x - 1, cell.position.y - 1)); // bottom left
            Flood(GetCell(cell.position.x + 1, cell.position.y - 1)); // bottom right
            Flood(GetCell(cell.position.x - 1, cell.position.y + 1)); // top left
            Flood(GetCell(cell.position.x + 1, cell.position.y + 1)); // top right
        }
    }

    private void Explode(Cell cell) // called when bomb tile is flipped
    {
        gameOver = true;

        cell.revealed = true;
        cell.exploded = true;
        state[cell.position.x, cell.position.y] = cell;

        for (int x = 0; x < width; x++) // loop through and reveal all mine tiles on board
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++) // check each non-mine tile and see if its flipped
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed) // unrevealed non-mine tile
                {
                    return;
                }
            }
        }

        gameOver = true;

        for (int x = 0; x < width; x++) // loop through and flag all mine tiles on board
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        gameOver = false;

        GenerateCells();
        // GenerateMines();
        // GenerateNumbers();

        firstClick = true;

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10); // making sure camera is in middle of board
        SetupBackground();
        board.Draw(state);
    }

    private void SetupBackground()
    {
        background.transform.position = new Vector3(width / 2f, height / 2f, 0f); // making sure background is aligned with board
        background.transform.localScale = new Vector3(width + (borderThickness * 2), height + (borderThickness * 2), 1);
    }

    private void GenerateCells() // Sets all cells in range to empty
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines(Cell cell) // places mines onto board
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (state[x, y].type == Cell.Type.Mine || InStartRange(cell, state[x, y])) // Loop makes sure we don't place a mine on a tile twice
            {
                x++;

                if (x >= width)
                {
                    x = 0;
                    y++;

                    if (y >= height)
                    {
                        y = 0;
                    }
                }
            }

            state[x, y].type = Cell.Type.Mine;
        }
    }

    private bool InStartRange(Cell startCell, Cell checkCell) // makes sure first tile user flips is a empty cell
    {
        if (checkCell.position == startCell.position 
            || checkCell.position == GetCell(startCell.position.x - 1, startCell.position.y).position 
            || checkCell.position == GetCell(startCell.position.x + 1, startCell.position.y).position
            || checkCell.position == GetCell(startCell.position.x, startCell.position.y - 1).position
            || checkCell.position == GetCell(startCell.position.x, startCell.position.y + 1).position
            || checkCell.position == GetCell(startCell.position.x + 1, startCell.position.y + 1).position
            || checkCell.position == GetCell(startCell.position.x + 1, startCell.position.y - 1).position
            || checkCell.position == GetCell(startCell.position.x - 1, startCell.position.y + 1).position
            || checkCell.position == GetCell(startCell.position.x - 1, startCell.position.y - 1).position)
        {
            return true;
        }

        return false;
    }

    private void GenerateNumbers() // sets count of cell to number of mines surrounding it
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0)
                {
                    cell.type = Cell.Type.Number;
                }

                state[x, y] = cell;
            }
        }
    }

    private int CountMines(int cellX, int cellY) // returns the number of mines surrounding cell
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) // current cell being checked, we skip
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (GetCell(x, y).type == Cell.Type.Mine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private int CountFlags(int cellX, int cellY) // returns the number of flags surrounding cell
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) // current cell being checked, we skip
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (GetCell(x, y).flagged == true)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool AnyCellsUnrevealed(int cellX, int cellY) // returns true if there is a surrounding unrevealed cell
    {
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) // current cell being checked, we skip
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (GetCell(x, y).revealed == false && !GetCell(x, y).flagged)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private Cell GetCell(int x, int y) // returns corresponding cell if valid, otherwise returns new cell with invalid type
    {
        if (IsValid(x, y))
        {
            return state[x, y];
        }
        else
        {
            return new Cell();
        }
    }

    private bool IsValid(int x, int y) // checks if cell is valid
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
