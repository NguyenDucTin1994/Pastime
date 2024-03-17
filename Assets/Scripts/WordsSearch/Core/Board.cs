using Chess;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WordsSearch
{
    public enum BoardType
    {
        x8,
        x9,
        x10,
    }
    public class Board
    {
        public char defaultChar;
        public WordDirection[] wordDirecs;
        public Cell[,] cells;
        public int size; // standard size is 8
        public BoardType boardType;

        public Board(BoardType _boardType)
        {
            defaultChar = ' ';
            boardType = _boardType;
            size = 8+ (int)_boardType;
            cells = new Cell[size, size];
            wordDirecs = new WordDirection[4] { WordDirection.South, WordDirection.East, WordDirection.SouthEast, WordDirection.NorthEast };

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j] = new Cell();
                    cells[i, j].letter = defaultChar;
                    cells[i, j].position = new Vector2Int(i, j);
                }
            }
        }


        public static bool IsInBoard(Vector3 mousePos) // standard 
        {
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);

            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        public static bool IsInBoard(Vector3 mousePos, int size) //example scale equal 9/8 or 10/8
        {
            Vector3 tempt = mousePos * size/ 8f;

            int x = Mathf.RoundToInt(tempt.x);
            int y = Mathf.RoundToInt(tempt.y);

            return x >= 0 && x < size && y >= 0 && y < size;
        }

        public static bool IsInBoardInt(Vector2Int mousePos) //standard
        {
            int x = mousePos.x;
            int y = mousePos.y;

            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        public static bool IsInBoardInt(Vector2Int mousePos, int _size) //example size equal 9 or 10
        {
            int x = mousePos.x;
            int y = mousePos.y;

            return x >= 0 && x < _size && y >= 0 && y < _size;
        }

        public static Vector3Int ConvertWorldPosToBoardPos(Vector3 mousePos) // standard
        {
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);

            return new Vector3Int(x, y, 0);
        }

        public static Vector3Int ConvertWorldPosToBoardPos(Vector3 mousePos, int size) //example scale equal 9/8 or 10/8
        {
            Vector3 tempt = mousePos *size/ 8f;

            return ConvertWorldPosToBoardPos(tempt);
        }


        public static bool IsLegalLine(Vector3Int startPos, Vector3Int endPos)
        {
            var delta = endPos - startPos;

            return (delta.x == 0) || (delta.y == 0) || (Mathf.Abs(delta.x) == Mathf.Abs(delta.y));

        }


        #region USE FOR PUT NEW WORD TO BOARD

        public void AddWordToBoard(WordInBoard _word)
        {
            for (int i = 0; i < _word.length; i++)
            {
                Vector2Int wordDirec = WordInBoard.GetValueOfDicrection(_word.wordDirection);

                int x = _word.startPos.x + wordDirec.x * i;
                int y = _word.startPos.y + wordDirec.y * i;

                this.cells[x, y].letter = _word.letters[i];
            }
        }

        #region   Flood Fill agorith and stop when make any legal WordInboard (random)
        public WordInBoard ChooseWordInBoard(string word)
        {
            WordInBoard chosenWordInBoard = null;

            int startRandomIndex = Random.Range(0, this.size*this.size);
            //int startRandomIndex =0;

            Vector2Int temt = MathFunction.ConvertListIndexToMatrixIndex(startRandomIndex, size);
            Vector2Int startPos = new Vector2Int(temt.y, temt.x);

            #region FLOOD  FILL ALGORITH

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visitedCells = new HashSet<Vector2Int>();

            queue.Enqueue(startPos);

            while (queue.Count > 0)
            {
                Vector2Int currentStartPos = queue.Dequeue();
                visitedCells.Add(currentStartPos);

                if (MakeLegalWordInBoard(word, currentStartPos, out chosenWordInBoard))
                {
                    return chosenWordInBoard;
                }

                Vector2Int[] neighbours =
                {
                    new Vector2Int(currentStartPos.x-1, currentStartPos.y),
                    new Vector2Int(currentStartPos.x+1, currentStartPos.y),
                    new Vector2Int(currentStartPos.x, currentStartPos.y-1),
                    new Vector2Int(currentStartPos.x, currentStartPos.y+1),

                };

                foreach (Vector2Int neighbour in neighbours)
                {
                    if (IsInBoardInt(neighbour) && !visitedCells.Contains(neighbour))
                        queue.Enqueue(neighbour);
                }
            }

            #endregion
            return chosenWordInBoard;


        }

        public bool MakeLegalWordInBoard(string word, Vector2Int startPos, out WordInBoard wordInBoard)
        {
            var letters = word.ToCharArray();

            RandomFunction.SuffleAllArr(wordDirecs);

            for (int i = 0; i < wordDirecs.Length; i++)
            {
                wordInBoard = new WordInBoard(word, new Vector2Int(startPos.x, startPos.y), wordDirecs[i]);

                if (IsLegalWordInBoard(wordInBoard))
                {
                    return true;
                }
            }

            wordInBoard = null;
            return false;
        }

        public bool IsLegalWordInBoard(WordInBoard wordInBoard)
        {
            if (!IsInBoardInt(wordInBoard.startPos) || !IsInBoardInt(wordInBoard.endPos))
            {
                //Debug.Log("out of currentBoard border");
                return false;
            }

            for (int i = 0; i < wordInBoard.length; i++)
            {
                Vector2Int wordDirec = WordInBoard.GetValueOfDicrection(wordInBoard.wordDirection);

                int x = wordInBoard.startPos.x + wordDirec.x * i;
                int y = wordInBoard.startPos.y + wordDirec.y * i;

                if (this.cells[x, y].letter != this.defaultChar && this.cells[x, y].letter != wordInBoard.letters[i])
                {
                    //Debug.Log("already have different char");
                    return false;
                }

            }

            return true;
        }
        #endregion

        #region Make full possible WordInBoard and choose the best

        public WordInBoard ChooseBestWordInBoard(string _word)
        {
            WordInBoard _chosenWordInBoard = null;

            var _letters = _word.ToCharArray();
            RandomFunction.SuffleAllArr(this.wordDirecs);

            List<int> _binaryWordsList = new List<int>();

            for (int i = 0; i < cells.GetLength(0); i++)  // X axis
            {
                for (int j = 0; j < cells.GetLength(1); j++) // Y axis
                {
                    Vector2Int _startPos = new Vector2Int(i, j);
                    if (!IsInBoardInt(_startPos))
                        continue;

                    for (int k = 0; k < this.wordDirecs.Length; k++) // 4 direc
                    {
                        Vector2Int _valueOfDirec = WordInBoard.GetValueOfDicrection(this.wordDirecs[k]);
                        Vector2Int _endPos = _startPos + (_word.Length - 1) * _valueOfDirec;
                        if (!IsInBoardInt(_endPos))
                            continue;

                        int _countDuplicateChar = 0;
                        bool isValid = true;

                        for (int t = 0; t < _letters.Length; t++) // letters in word
                        {
                           
                            int x = _startPos.x + t * _valueOfDirec.x;
                            int y = _startPos.y + t * _valueOfDirec.y;

                            if (cells[x, y].letter == _letters[t])
                            {
                                _countDuplicateChar++;
                            }
                            else
                            {
                                if (cells[x, y].letter != this.defaultChar)// letter in currentBoard # default and letter in word => stop loop, next direc
                                {
                                    isValid = false;
                                    break;
                                }
                                   
                            }
                        }

                        if (isValid)
                        {
                            _binaryWordsList.Add(BinaryWordInBoard.MakeBinaryWordValue(i * this.size + j, (int)wordDirecs[k], _countDuplicateChar));
                            
                        }

                        

                        
                    }

                }
            }

            if(_binaryWordsList.Count > 0)
            {
                int max = _binaryWordsList.Max();
                //Debug.Log("max" + BinaryWordInBoard.GetCompareValue(max));
                
                if (BinaryWordInBoard.GetCompareValue(max)==0)
                {
                    
                    max = _binaryWordsList[Random.Range(0, _binaryWordsList.Count)];
                }

                
                _chosenWordInBoard = BinaryWordInBoard.GetWordInBoard(_word, max, this.size);
            }
           


            return _chosenWordInBoard;
        }


        #endregion


        public void FillAllBoardRandom()
        {
            for(int i=0; i<this.size; i++)
            {
                for(int j=0; j<this.size; j++)
                {
                    if (cells[i,j].letter == this.defaultChar)
                    {
                        cells[i,j].letter = Alphabet.letters[Random.Range(0,Alphabet.letters.Length)];
                    }
                }
            }
        }



        public void ResetBoard()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j].letter = defaultChar;
                }
            }
        }
        #endregion

    }
}

