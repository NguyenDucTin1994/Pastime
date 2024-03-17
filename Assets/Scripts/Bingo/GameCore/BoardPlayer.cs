using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bingo
{
    public class BoardPlayer : Board
    {
        public List<ArchievedPrize> archievedPrizes;


        public bool bingoSuccess;
        public override void Start()
        {
            archievedPrizes = new List<ArchievedPrize>();

            CallerNumberController.instance.CallNumber += PlayerUpdateCalledNumber;
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            if (bingoGrid != null)
            {
                InstantiateCellsUI();

            }
        }

        public void PlayerUpdateCalledNumber(int _newNumber)
        {
            int index = Array.BinarySearch(cells, new Cell { value = _newNumber }, new CellComparer());

            if (index >= 0)
            {
                cells[index].isCalled = true;
                int pos = cells[index].position.x * 5 + cells[index].position.y;

                if (bingoGrid != null)
                {
                    DisPlayUpdateCalledNumber(bingoGrid.cellUIs[pos]);
                }

                bingoPrize++;

                PlayerInternalCheckAllPrize(cells[index].position);
            };
        }

        #region INTERNAL CHECK IN PLAYER BOX

        #endregion
        public void PlayerInternalCheckAllPrize(Vector2Int pos)
        {
            PlayerCheckHorizontalPrize(pos);
            PlayerCheckVerticalPrize(pos);
            PlayerCheckDiagonalPrize(pos);
            PlayerCheckCornerPrize(pos);
            PlayerCheckEdgePrize(pos);
            PlayerCheckBingoPrize();
        }
        public void PlayerCheckHorizontalPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Row] == true && getPrizes[(int)Prize.Row] == false)
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
                    archievedPrizes.Add(new ArchievedPrize(pos, Prize.Row));
                }
            }
        }


        public void PlayerCheckVerticalPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Column] == true && getPrizes[(int)Prize.Column] == false)
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
                    archievedPrizes.Add(new ArchievedPrize(pos, Prize.Column));

                }
            }
        }

        public void PlayerCheckDiagonalPrize(Vector2Int pos)
        {

            if (BingoGameController.instance.havingPrizes[(int)Prize.Diagonal] == true && getPrizes[(int)Prize.Diagonal] == false)
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
                        archievedPrizes.Add(new ArchievedPrize(pos, Prize.Diagonal));
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

                        archievedPrizes.Add(new ArchievedPrize(pos, Prize.Diagonal));

                    }
                }
            }
        }

        public void PlayerCheckCornerPrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Corners] == true && getPrizes[(int)Prize.Corners] == false)
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

                        archievedPrizes.Add(new ArchievedPrize(pos, Prize.Corners));


                    }
                }
            }
        }

        public void PlayerCheckEdgePrize(Vector2Int pos)
        {
            if (BingoGameController.instance.havingPrizes[(int)Prize.Outline] && !getPrizes[(int)Prize.Outline])
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

                    archievedPrizes.Add(new ArchievedPrize(pos, Prize.Outline));
                }
            }
        }
        public void PlayerCheckBingoPrize()
        {
            if (bingoPrize >= 24)
            {
                getPrizes[(int)Prize.BINGO] = true;
                archievedPrizes.Add(new ArchievedPrize(Vector2Int.zero, Prize.BINGO));
            }
        }


        // Change color image of cells when it is a part of column or row to get prize
        public void DisPlayCellsPrize(ArchievedPrize _archievedPrize)
        {
            var pos = _archievedPrize.pos;
            switch (_archievedPrize.prize)
            {
                case Prize.Row:
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(pos.x, col), 5)]);
                        }
                    }
                    break;

                case Prize.Column:
                    {
                        for (int row = 0; row < 5; row++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(row, pos.y), 5)]);
                        }
                    }
                    break;

                case Prize.Diagonal:
                    {
                        if (pos.x == pos.y)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(i, i), 5)]);
                            }
                        }
                        else if (pos.x + pos.y == 4)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(i, 4 - i), 5)]);
                            }
                        }
                    }
                    break;

                case Prize.Corners:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2Int checkPos = MathFunction.ConvertListIndexToMatrixIndex(i, 2) * 4;
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(checkPos, 5)]);
                        }
                    }
                    break;

                case Prize.Outline:
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(0, i), 5)]);
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(i, 0), 5)]);
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(0, 4), 5)]);
                            DisPlayCellsPrize(bingoGrid.cellUIs[MathFunction.ConvertMatrixIndexToListIndex(new Vector2Int(4, i), 5)]);
                        }
                    }
                    break;

                case Prize.BINGO:
                    {
                        for (int i = 0; i < bingoGrid.cellUIs.Length; i++)
                        {
                            DisPlayCellsPrize(bingoGrid.cellUIs[i]);
                        }
                    }
                    break;


            }

            // fix for center star
            bingoGrid.cellUIs[12].image.color = Color.white;

        }

        public override IEnumerator GetPrize(Prize prize)
        {
            var _prizeWinner = new PrizeWinner(this, prize);

            PrizeManager.instance.AddPrize(_prizeWinner);
            yield return null;

            BingoGameController.instance.havingPrizes[(int)prize] = false;
        }


        // Check Prize manual by click BINGO button
        public void BingoClick()
        {
            int countPrizes = archievedPrizes.Count;

            for (int i = 0; i < archievedPrizes.Count; i++)
            {
                ArchievedPrize _archievedPrize = archievedPrizes[i];

                if (BingoGameController.instance.havingPrizes[(int)_archievedPrize.prize] == true)
                {
                    bingoSuccess = true;
                    BingoGameController.instance.PlayerGetPrize(_archievedPrize.prize);
                    StartCoroutine(GetPrize(_archievedPrize.prize));
                    DisPlayCellsPrize(_archievedPrize);
                }
                else
                {
                    countPrizes--;
                }
            }

            if (countPrizes <= 0)
            {
                CanvasManager.instance.TurnOnCanvasInDuration(CanvasType.DontHavePrize, 1.5f);
                bingoSuccess = false;// for BingoController ( punish when bingo fail)
            }

            if (bingoSuccess)
                SoundManager.instance.PlaySFXSound(SFXSound.Correct);
            else
                SoundManager.instance.PlaySFXSound(SFXSound.Incorrect);

            PrizeManager.instance.UpdateUIPrizeWinner(26);// any number


        }


    }

    // INFORMATION FOR ARCHIEVED PRIZE( BY PLAYER INTERNAL CHECK) 
    [Serializable]
    public class ArchievedPrize
    {
        public Vector2Int pos;
        public Prize prize;
        public ArchievedPrize(Vector2Int pos, Prize prize)
        {
            this.pos = pos;
            this.prize = prize;
        }
    }
}

