using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WordsSearch
{
    public class DrawLineController : MonoBehaviour
    {
        public static event Action<int> DetectWordEvent;

        [SerializeField] private WordsSearchData data;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BoardController boardController;



        [SerializeField] LineRenderer[] lines;
        [SerializeField] private LineRenderer currentLineRenderer;
        [SerializeField] private int currentLineIndex;

        private Vector3Int startLine;
        private Vector3Int endLine;
        [SerializeField] private Vector3Int resetPoint = Vector3Int.one * 100;

        bool isMousePressed;
        bool isPlaying=false;

        [SerializeField] private int numberOfWords = 9;

        private void Start()
        {
            isPlaying = false;
            InstatiateLines();
            GameManager.NewGameEvent += OnNewGame;
        }

        private void Update()
        {
            DrawLine();
            //ShowScreenPosAndWorldPosOfMouseClick();
        }


        public void ShowScreenPosAndWorldPosOfMouseClick()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Debug.Log("mousePos" + mousePos);
                mousePos.z = Camera.main.nearClipPlane; // Set z-coordinate to near clip plane
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                Debug.Log(worldPos);
                //Debug.Log(Input.endLine);
            }
        }

        public void DrawLine()
        {
            if(isPlaying)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    if (Board.IsInBoard(Camera.main.ScreenToWorldPoint(Input.mousePosition), boardController.Size))
                    {
                        isMousePressed = true;
                        startLine = Board.ConvertWorldPosToBoardPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), boardController.Size);

                        currentLineRenderer.positionCount = 2;
                        currentLineRenderer.SetPosition(0, (Vector3)startLine * 8f / boardController.Size); //draw currentBoard grid point
                        currentLineRenderer.SetPosition(1, (Vector3)startLine * 8f / boardController.Size);
                    }

                }

                if (Input.GetMouseButtonUp(0))
                {
                    //Debug.Log( startLine+"     "+endLine+   WordDrawByLine(startLine,endLine));

                    ResetLinePosition(currentLineRenderer);
                    isMousePressed = false;
                }

                if (isMousePressed)
                {
                    if (Board.IsInBoard(Camera.main.ScreenToWorldPoint(Input.mousePosition), boardController.Size))
                    {
                        endLine = Board.ConvertWorldPosToBoardPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), boardController.Size);
                        if (Board.IsLegalLine(startLine, endLine))
                        {
                            int wordIndex;
                            currentLineRenderer.SetPosition(1, (Vector3)endLine * 8f / boardController.Size);

                            if (CheckWordMakeByLine(startLine, endLine, out wordIndex))
                            {
                                isMousePressed = false;
                                DetectWord(wordIndex);
                                SoundManager.instance.PlaySFXSound(SFXSound.Correct);
                                NextLine();
                            }
                        }


                    }

                }
            }
            

        }

        public bool CheckWordMakeByLine(Vector3Int startPos, Vector3Int endPos, out int wordIndex)
        {
            string wordDrawByLine = WordDrawByLine(startPos,endPos);
            char[] charArray = wordDrawByLine.ToCharArray();
            Array.Reverse(charArray);
            string invertWord =new string(charArray);


            wordIndex = -1;
            if (gameManager.wordSearchDiction.ContainsKey(wordDrawByLine))
            {
                //Debug.Log(wordDrawByLine);
                wordIndex = gameManager.wordSearchDiction[wordDrawByLine];
                gameManager.wordSearchDiction.Remove(wordDrawByLine);
                return true;
            }
            else if (gameManager.wordSearchDiction.ContainsKey(invertWord))
            {
                wordIndex = gameManager.wordSearchDiction[invertWord];
                gameManager.wordSearchDiction.Remove(invertWord);
                return true;
            }
            else
                return false;
        }

        public string WordDrawByLine(Vector3Int startPos, Vector3Int endPos)
        {
            string wordDraw = "";

            Vector3Int delta = endPos - startPos;
            
            int max = (Math.Abs(delta.x) > Math.Abs(delta.y)) ? Math.Abs(delta.x) : Math.Abs(delta.y);

            //Debug.Log(delta+ " " + max);
            if (max != 0)
            {
                delta = delta / max;
            }
            //Debug.Log(delta + " " + max);

            for (int i = 0; i < max+1 ; i++)
            {
                int x = startPos.x + delta.x * i;
                int y = startPos.y + delta.y * i;

                char charTempt = boardController.currentBoard.cells[x, y].letter;
                wordDraw += charTempt.ToString();
            }

            return wordDraw;

        }

        public void DetectWord(int wordIndex)
        {
            DetectWordEvent?.Invoke(wordIndex);
        }
        public void InstatiateLines()
        {
            lines = new LineRenderer[numberOfWords];
            GameObject LineContainer = new GameObject("LineContainer");
            for (int i = 0; i < numberOfWords; i++)
            {
                GameObject lineObject = Instantiate(data.Line, LineContainer.transform) as GameObject;
                LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

                lineRenderer.startColor = data.LineColors[i];
                lineRenderer.endColor = data.LineColors[i];

                ResetLinePosition(lineRenderer);

                lines[i] = lineRenderer;
            }
        }

        public void ResetLinePosition(LineRenderer lineRenderer)
        {
            lineRenderer.SetPosition(0, resetPoint);
            lineRenderer.SetPosition(1, resetPoint);
            lineRenderer.widthMultiplier = 1 * 8 /(float) boardController.Size;
        }
        public void OnNewGame(string[] args)
        {
            isPlaying = true;
            currentLineIndex = 0;
            currentLineRenderer = lines[0];
            for(int i = 0;i < lines.Length;i++)
            {
                ResetLinePosition((LineRenderer)lines[i]);
            }
        }

        public void NextLine()
        {

            currentLineIndex += 1;

            if (currentLineIndex > numberOfWords - 1)
            {
                currentLineIndex = numberOfWords-1;
                isPlaying = false;
                SoundManager.instance.PlaySFXSound(SFXSound.Win);
                gameManager.EndGame();
                
            }

            currentLineRenderer = lines[currentLineIndex];

        }
    }
}
