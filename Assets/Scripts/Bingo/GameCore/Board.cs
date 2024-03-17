using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Bingo
{
    public enum BoardOwner
    {
        Player,
        Boss1,
        Boss2,
        Boss3,
    }

    public enum Prize
    {
        Row,
        Column,
        Diagonal,
        Corners,
        Outline,
        BINGO
    }

    public class Board : MonoBehaviour
    {
        public BoardOwner owner;
        public BingoGrid_AutoScale bingoGrid;

        public bool[] getPrizes;
        public int bingoPrize;

        public Cell[] cells = new Cell[25];
        public Cell[,] cellsTwoDimension = new Cell[5, 5];

        public ColumnInBoard columnB;
        public ColumnInBoard columnI;
        public ColumnInBoard columnN;
        public ColumnInBoard columnG;
        public ColumnInBoard columnO;

        public ColumnInBoard[] columnInBoards;

        public Color getPrizedColor;


        public void Awake()
        {
            columnB = new ColumnInBoard(ColumnType.B);
            columnI = new ColumnInBoard(ColumnType.I);
            columnN = new ColumnInBoard(ColumnType.N);
            columnG = new ColumnInBoard(ColumnType.G);
            columnO = new ColumnInBoard(ColumnType.O);

            columnInBoards = new ColumnInBoard[] { columnB, columnI, columnN, columnG, columnO };

            InstantiateCells();

            Array.Sort(cells, new CellComparer());

            bingoPrize = 0;

            getPrizes = new bool[6] { false, false, false, false, false, false };

        }

        public virtual void Start()
        {
            CallerNumberController.instance.CallNumber += UpdateCalledNumber;

            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            if (bingoGrid != null)
            {
                InstantiateCellsUI();

            }

        }
        public void InstantiateCells()
        {

            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 5; row++)
                {
                    int index = row * 5 + col;
                    cells[index] = new Cell();
                    cells[index].columnType = (ColumnType)col;
                    cells[index].position = new Vector2Int(row, col);
                    cellsTwoDimension[row, col] = cells[index];

                    cells[index].isCalled = false;
                    cells[index].value = columnInBoards[col].valuesPossible[row];

                    // cellsTwoDimension[row,col] = cells[index];
                }
            }

            // SPECIAL SETTING FOR CENTER CELL
            cells[12].value = 100;
            cells[12].isCalled = true;
        }



        public void UpdateCalledNumber(int _newNumber)
        {
            int index = Array.BinarySearch(cells, new Cell { value = _newNumber }, new CellComparer());

            if (index >= 0)
            {
                cells[index].isCalled = true;
                int pos = cells[index].position.x * 5 + cells[index].position.y;

                if (bingoGrid != null)
                {
                    DisPlayUpdateCalledNumber(bingoGrid.cellUIs[pos]);// virtual method 
                }

                bingoPrize++;

                CheckAllPrize(cells[index].position);
            };
        }

        #region CHECK VIETLOTT
        public void CheckAllPrize(Vector2Int pos)
        {
            CheckHorizontalPrize(pos);
            CheckVerticalPrize(pos);
            CheckDiagonalPrize(pos);
            CheckCornerPrize(pos);
            CheckEdgePrize(pos);
            CheckBingoPrize();
        }

        public void CheckHorizontalPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Row] == true)
            {
                for (int col = 0; col < 5; col++)
                {
                    getPrizes[(int)Prize.Row] = true;
                    if (cellsTwoDimension[pos.x, col].isCalled == false)
                    {
                        getPrizes[(int)Prize.Row] = false;
                        break;
                    }
                }

                if (getPrizes[(int)Prize.Row] == true)
                {

                    StartCoroutine(GetPrize(Prize.Row));

                    //display
                    if (bingoGrid != null)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(pos.x, col), 5)]);
                        }
                    }

                }
            }
        }

        public void CheckVerticalPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Column] == true)
            {
                for (int row = 0; row < 5; row++)
                {
                    getPrizes[(int)Prize.Column] = true;
                    if (cellsTwoDimension[row, pos.y].isCalled == false)
                    {
                        getPrizes[(int)Prize.Column] = false;
                        break;
                    }
                }

                if (getPrizes[(int)Prize.Column] == true)
                {

                    StartCoroutine(GetPrize(Prize.Column));

                    //display
                    if (bingoGrid != null)
                    {
                        for (int row = 0; row < 5; row++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(row, pos.y), 5)]);
                        }
                    }

                }
            }
        }

        public void CheckDiagonalPrize(Vector2Int pos)
        {

            if (BingoGameController.instance.havingPrizes[(int)Prize.Diagonal] == true)
            {
                // diagonal 1
                if (pos.x == pos.y)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        getPrizes[(int)Prize.Diagonal] = true;
                        if (cellsTwoDimension[i, i].isCalled == false)
                        {
                            getPrizes[(int)Prize.Diagonal] = false;
                            break;
                        }



                    }
                    if (getPrizes[(int)Prize.Diagonal] == true)
                    {

                        StartCoroutine(GetPrize(Prize.Diagonal));


                        //display
                        if (bingoGrid != null)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(i, i), 5)]);
                            }
                        }


                    }
                }

                // diagonal 2
                if (pos.x + pos.y == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        getPrizes[(int)Prize.Diagonal] = true;
                        if (cellsTwoDimension[i, 4 - i].isCalled == false)
                        {
                            getPrizes[(int)Prize.Diagonal] = false;
                            break;
                        }
                    }

                    if (getPrizes[(int)Prize.Diagonal] == true)
                    {

                        StartCoroutine(GetPrize(Prize.Diagonal));

                        //display
                        if (bingoGrid != null)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(i, 4 - i), 5)]);
                            }
                        }

                    }
                }
            }
        }

        public void CheckCornerPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Corners] == true)
            {
                if ((pos.x % 4 == 0) && (pos.y % 4 == 0))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2Int checkPos = MathFunction.ConvertListIndexToMatrixIndex(i, 2) * 4;
                        getPrizes[(int)Prize.Corners] = true;
                        if (cellsTwoDimension[checkPos.x, checkPos.y].isCalled == false)
                        {
                            getPrizes[(int)Prize.Corners] = false;
                            break;
                        }
                    }

                    if (getPrizes[(int)Prize.Corners] == true)
                    {

                        StartCoroutine(GetPrize(Prize.Corners));

                        //display
                        if (bingoGrid != null)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Vector2Int checkPos = MathFunction.ConvertListIndexToMatrixIndex(i, 2) * 4;
                                DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(checkPos, 5)]);
                            }
                        }
                    }
                }
            }
        }

        public void CheckEdgePrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Outline])
            {
                if ((pos.x % 4 == 0) || (pos.y % 4 == 0))
                {
                    getPrizes[(int)Prize.Outline] = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if (!cellsTwoDimension[0, i].isCalled || !cellsTwoDimension[i, 0].isCalled || !cellsTwoDimension[i, 4].isCalled || !cellsTwoDimension[4, i].isCalled)
                        {
                            getPrizes[(int)Prize.Outline] = false;
                            break;
                        }
                    }
                }

                if (getPrizes[(int)Prize.Outline] == true)
                {

                    StartCoroutine(GetPrize(Prize.Outline));
                }
            }
        }

        public void CheckBingoPrize()
        {
            if (bingoPrize >= 24)
            {
                getPrizes[(int)Prize.BINGO] = true;
                StartCoroutine(GetPrize(Prize.BINGO));
            }
        }




        public virtual IEnumerator GetPrize(Prize prize)
        {
            var _prizeWinner = new PrizeWinner(this, prize);

            PrizeManager.instance.AddPrize(_prizeWinner);
            yield return new WaitForEndOfFrame();
            BingoGameController.instance.havingPrizes[(int)prize] = false;
            BingoGameController.instance.PlayerCantGetPrize();
        }

        #endregion

        #region DISPLAY METHOD 
        public void InstantiateCellsUI()
        {
            for (int i = 0; i < 25; i++)
            {
                var pos = MathFunction.ConvertListIndexToMatrixIndex(i, 5);
                bingoGrid.cellUIs[i].text.text = cellsTwoDimension[pos.x, pos.y].value.ToString();
            }

            // Specical Display For Center Cell
            bingoGrid.cellUIs[12].text.text = "";

        }
        public void DisPlayUpdateCalledNumber(CellUIController _cellUIController)
        {
           // _cellUIController.image.color = Color.green;
        }

        public void DisPlayCellsPrize(CellUIController _cellUIController)
        {
            _cellUIController.image.color = getPrizedColor;
        }

        #endregion

        #region NEW GAME
        public void OnNewGame()
        {
            // Shuffle Value In DataBase of each column (B,I,N,G,O)
            for (int i = 0; i < columnInBoards.Length; i++)
            {
                RandomFunction.ShufflePartOfArr(columnInBoards[i].valuesPossible, 5);
            }



            // Instatiate new value for  cell
            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 5; row++)
                {

                    cellsTwoDimension[row, col].isCalled = false;
                    cellsTwoDimension[row, col].value = columnInBoards[col].valuesPossible[row];
                }
            }

            cellsTwoDimension[2, 2].value = 100;
            cellsTwoDimension[2, 2].isCalled = true;

            Array.Sort(cells, new CellComparer());

            // Display value in grid
            if (bingoGrid != null)
            {
                InstantiateCellsUI();

            }

            //Reset prize 
            for (int i = 0; i < getPrizes.Length; ++i)
            {
                getPrizes[i] = false;
            }

            bingoPrize = 0;

        }


        #endregion
    }

    public class ColumnInBoard
    {
        public ColumnType columnType;
        public int startValue;
        public int[] valuesPossible;

        public ColumnInBoard(ColumnType _columnType)
        {
            this.columnType = _columnType;
            this.startValue = 1 + (int)columnType * 15;
            this.valuesPossible = new int[15];

            for (int i = 0; i < 15; i++)
            {
                valuesPossible[i] = startValue + i;

            }

            RandomFunction.ShufflePartOfArr(this.valuesPossible, 5);

        }
    }

}
