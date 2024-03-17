using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;

namespace WordsSearch
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private WordsSearchData data;
        public Board currentBoard;
        public Board[] boards;

        [SerializeField] private int size = 8;
        public int Size => size;

        public float Scale => size/8f;

        [SerializeField]
        public GameObject[,] cellObjects;

        #region FOR CREATE BOARD CONTENT

        private string[] currentWords;
        [SerializeField] private BoardUIManager boardUIManager;

        #endregion

        private void Awake()
        {
            boards = new Board[3] {new Board(BoardType.x8), new Board(BoardType.x9), new Board(BoardType.x10)};
            SetNewBoardType(BoardType.x8);
        }


        private void Start()
        {
            GameManager.NewGameEvent += OnNewGame;
            //CreatBoard();
        }
        public void CreatBoard()
        {
            
            cellObjects = new GameObject[this.size, this.size];

            for (int i = 0; i < cellObjects.GetLength(0); i++)
            {
                for (int j = 0; j < cellObjects.GetLength(1); j++)
                {
                    GameObject newCellObject = Instantiate(data.CellObject) as GameObject;
                    newCellObject.transform.position = new Vector3(i, j, 0) * 1/this.Scale;
                    newCellObject.transform.localScale = Vector3.one * 1/this.Scale;
                    newCellObject.transform.SetParent(transform, false);

                    //if ((i + j) % 2 == 0) newCellObject.GetComponent<SpriteRenderer>().color = data.LineColors[2];
                    newCellObject.GetComponent<SpriteRenderer>().color = Color.black;
                    if ((i + j) % 2 == 0) newCellObject.GetComponent<SpriteRenderer>().color = Color.white;
                    cellObjects[i, j] = newCellObject;
                }
            }

        }

        #region CREATE BOARD CONTENT
        public void CreateBoardContent(string[] _wordContent)
        {
            bool finishFillWord = false;

            currentWords= _wordContent.OrderBy(x => -x.Length).ToArray();

            for(int count =0; count<5; count++)
            {
                if (!finishFillWord)
                {
                    //Debug.Log("count" + count);
                    for (int i = 0; i < currentWords.Length; i++)
                    {
                        WordInBoard wordInBoard;

                        wordInBoard = this.currentBoard.ChooseBestWordInBoard(currentWords[i]);

                        if (wordInBoard == null)
                        {
                            currentBoard.ResetBoard();
                            break;
                        }

                        this.currentBoard.AddWordToBoard(wordInBoard);

                        if(i== currentWords.Length-1)
                        {
                            finishFillWord = true;
                        }
                    }
                }

            }
           



            if (!finishFillWord)
            {
                SimpleCreateBoardContent();
            }

            this.currentBoard.FillAllBoardRandom();
            this.boardUIManager.CurrentBoardUI. UpdateBoard(this.currentBoard);
        }

        public void SimpleCreateBoardContent()
        {
            for (int i = 0; i < currentWords.Length; i++)
            {
                WordInBoard wordInBoard;

                if (i < currentWords.Length - 1)
                    wordInBoard = new WordInBoard(currentWords[i], new Vector2Int(0, i), WordDirection.East);
                else
                    wordInBoard = new WordInBoard(currentWords[i], new Vector2Int(this.size-1, this.size-1), WordDirection.South);
                this.currentBoard.AddWordToBoard(wordInBoard);
            }

        }

        public void ResetBoard()
        {
            //Debug.Log("reset");
            this.currentBoard.ResetBoard();
            this.boardUIManager.CurrentBoardUI. UpdateBoard(this.currentBoard);
        }
        #endregion



        #region NEW GAME

        private void OnNewGame(string[] _wordContent)
        {
            ResetBoard();
            CreateBoardContent(_wordContent);
        }
        #endregion

        public void SetNewBoardType(BoardType type)
        {
            SetNewBoard((int)type);
            
        }
        public void SetNewBoard(int newBoardType)
        {
            this.size = 8 + newBoardType;
            currentBoard = boards[newBoardType];
            boardUIManager.TurnOnCurrentBoardUI((BoardType) newBoardType);
        }
        public void Test()
        {
            var test = "12345678";


            var wordinBoard = currentBoard.ChooseBestWordInBoard(test);

            wordinBoard.ShowDebug();
            currentBoard.AddWordToBoard(wordinBoard);

            this.boardUIManager.CurrentBoardUI. UpdateBoard(this.currentBoard);
        }
    }
}

