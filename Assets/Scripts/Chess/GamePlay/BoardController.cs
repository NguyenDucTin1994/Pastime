using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chess
{
    public class BoardController : MonoBehaviour
    {

        public PieceTheme pieceTheme;
        public BoardTheme boardTheme;
        public ChessData data;
        public GameObject legalMovePrefab;


        public bool showValidMove;

        public bool whiteIsBottom = true;

        public float moveAnimDuration = 0.15f;


        public GameObject[,] squareObjects;
        public GameObject[,] squarePieceObjects;
        public GameObject[,] legalMoveObjects;

        Move lastMadeMove;
        MoveGenerator moveGenerator;

        public float pieceDepth = 1f;
        public float pieceDragDepth = 0f;

        public float scaleFactor = 1;

        void Awake()
        {
            moveAnimDuration = 0.15f;
            moveGenerator = new MoveGenerator();
            CreateBoard();
        }

        private void Start()
        {
          
        }

        public void SetColorValidTargetOfMove(Board board, Coord fromSquare)
        {
            if (showValidMove)
            {

                var moves = moveGenerator.GenerateMoves(board);

                for (int i = 0; i < moves.Count; i++)
                {
                    Move move = moves[i];

                    if (move.StartSquare == BoardRepresentation.IndexFromCoord(fromSquare))
                    {
                        Coord coordLegalMove = BoardRepresentation.CoordFromIndex(move.TargetSquare);
                        legalMoveObjects[coordLegalMove.fileIndex, coordLegalMove.rankIndex].SetActive(true);
                    }
                }
            }
        }

        public void DragPiece(Coord pieceCoord, Vector3 mousePos)
        {
            squarePieceObjects[pieceCoord.fileIndex, pieceCoord.rankIndex].transform.position = new Vector3(mousePos.x, mousePos.y, pieceDragDepth * scaleFactor );
        }

        public void ResetPiecePosition(Coord pieceCoord)
        {
            Vector3 pos = PositionFromCoord(pieceCoord.fileIndex, pieceDepth, pieceCoord.rankIndex);
            squarePieceObjects[pieceCoord.fileIndex, pieceCoord.rankIndex].transform.position = pos;
        }

        public void SelectSquare(Coord coord)
        {
            SetSquareColour(coord, boardTheme.lightSquares.selected, boardTheme.darkSquares.selected);
        }

        public void DeselectSquare(Coord coord)
        {
            //BoardTheme.SquareColours colours = (coord.IsLightSquare ()) ? boardTheme.lightSquares : boardTheme.darkSquares;
            //squareMaterials[coord.file, coord.rank].color = colours.normal;
            ResetSquareColours();
        }

        public bool TryGetValidCoord(Vector3 screenPos, out Coord coord)
        {

            int file = Mathf.RoundToInt(screenPos.x / scaleFactor);
            int rank = Mathf.RoundToInt(screenPos.y / scaleFactor);
            if (!whiteIsBottom)
            {
                file = 7 - file;
                rank = 7 - rank;
            }
            coord = new Coord(file, rank);
            return file >= 0 && file < 8 && rank >= 0 && rank < 8;
        }

        // Tạo và xóa các quân cờ trong Piece
        public void UpdatePosition(Board board, bool newGame = false)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Coord coord = new Coord(file, rank);
                    int piece = board.Square[BoardRepresentation.IndexFromCoord(coord.fileIndex, coord.rankIndex)];

                    if (newGame)
                    {
                        int childs = squarePieceObjects[file, rank].transform.childCount;
                        for (int i = 0; i < childs; i++)
                        {
                            Destroy(squarePieceObjects[file, rank].transform.GetChild(i).gameObject);
                        }

                        if (pieceTheme.GetPiecePrefab(piece) != null)
                        {
                            GameObject newPiece = Instantiate(pieceTheme.GetPiecePrefab(piece));
                            newPiece.transform.parent = squarePieceObjects[file, rank].transform;
                            newPiece.transform.localPosition = Vector3.zero;
                        }
                    }
                    else
                    {
                        if (pieceTheme.GetPiecePrefab(piece) != null)
                        {
                            int childs = squarePieceObjects[file, rank].transform.childCount;
                            if (childs > 1)
                            {
                                Destroy(squarePieceObjects[file, rank].transform.GetChild(0).gameObject);
                            }
                           
                        }
                        else
                        {
                            int childs = squarePieceObjects[file, rank].transform.childCount;
                            if (childs > 1)
                            {
                                Destroy(squarePieceObjects[file, rank].transform.GetChild(0).gameObject);
                            }
                        }
                    }
                }
            }

        }

        public void Castling(Board board, Move move)
        {
            int startCoordFile = BoardRepresentation.CoordFromIndex(move.StartSquare).fileIndex;
            int targetCoordFile = BoardRepresentation.CoordFromIndex(move.TargetSquare).fileIndex;

            if (startCoordFile > targetCoordFile)
            {
                Debug.Log("QueenSide Castling! ");
                StartCoroutine(AnimateMove(new Move(move.TargetSquare - 2, move.StartSquare - 1, Move.Flag.None), board)); ;
            }
            else
            {
                Debug.Log("KingSide Castling! ");
                StartCoroutine(AnimateMove(new Move(move.TargetSquare + 1, move.StartSquare + 1, Move.Flag.None), board));
            }
        }

        public void Promotion(Board board, Move move)
        {
            Coord coord = BoardRepresentation.CoordFromIndex(move.TargetSquare) ; 
            Transform piece = squarePieceObjects[coord.fileIndex, coord.rankIndex].transform;

            Destroy(piece.GetChild(0).gameObject);

            int pieceInt = board.Square[BoardRepresentation.IndexFromCoord(coord.fileIndex, coord.rankIndex)];


            GameObject newPiece = Instantiate(pieceTheme.GetPiecePrefab(pieceInt));
            newPiece.transform.parent = piece;
            newPiece.transform.localPosition = Vector3.zero;
        }

        public void OnDoMove(Board board, Move move, bool animate = false)
        {

            if (move.MoveFlag == 2) //castling
            {
                Castling(board, move);
            }

            lastMadeMove = move;

            if (animate)
            {
                StartCoroutine(AnimateMove(move, board));
            }
            else
            {
                Coord startCoord = BoardRepresentation.CoordFromIndex(move.StartSquare);
                Coord targetCoord = BoardRepresentation.CoordFromIndex(move.TargetSquare);

                Transform pieceT = squarePieceObjects[startCoord.fileIndex, startCoord.rankIndex].transform.GetChild(0);

                pieceT.transform.parent = squarePieceObjects[targetCoord.fileIndex, targetCoord.rankIndex].transform;
                pieceT.transform.localPosition = Vector3.zero;

                if (move.MoveFlag >= 3 && move.MoveFlag <= 6) //promotion
                {
                    Promotion(board, move);
                    //Debug.Log("flag"+move.MoveFlag);
                }

                UpdatePosition(board);
                ResetSquareColours();
            }
        }

        IEnumerator AnimateMove(Move move, Board board)
        {
            float t = 0;
            Coord startCoord = BoardRepresentation.CoordFromIndex(move.StartSquare);
            Coord targetCoord = BoardRepresentation.CoordFromIndex(move.TargetSquare);
            Transform pieceT = squarePieceObjects[startCoord.fileIndex, startCoord.rankIndex].transform.GetChild(0);
            Vector3 startPos = PositionFromCoord(startCoord, pieceDepth);
            Vector3 targetPos = PositionFromCoord(targetCoord, pieceDepth);
            SetSquareColour(BoardRepresentation.CoordFromIndex(move.StartSquare), boardTheme.lightSquares.moveFromHighlight, boardTheme.darkSquares.moveFromHighlight);

            if (pieceT != null)
            {
                while (t <= 1)
                {
                    yield return null;
                    t += Time.deltaTime * 1 / moveAnimDuration;
                    pieceT.position = Vector3.Lerp(startPos, targetPos, t);
                }


                pieceT.transform.parent = squarePieceObjects[targetCoord.fileIndex, targetCoord.rankIndex].transform;
                pieceT.transform.localPosition = Vector3.zero;

                if (move.MoveFlag >= 3 && move.MoveFlag <= 6) //promotion
                {
                    Promotion(board, move);
                }

                UpdatePosition(board);
                ResetSquareColours();
            }
            
        }

        void HighlightMove(Move move)
        {
            SetSquareColour(BoardRepresentation.CoordFromIndex(move.StartSquare), boardTheme.lightSquares.moveFromHighlight, boardTheme.darkSquares.moveFromHighlight);
            
             SetSquareColour(BoardRepresentation.CoordFromIndex(move.TargetSquare), boardTheme.lightSquares.moveToHighlight, boardTheme.darkSquares.moveToHighlight);


        }

        void CreateBoard()
        {

            // Shader squareShader = Shader.Find("Unlit/Color");
            squareObjects = new GameObject[8, 8];
            squarePieceObjects = new GameObject[8, 8];
            legalMoveObjects = new GameObject[8, 8];

            var legalMoveContainer = new GameObject("Legal Container");

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                   

                    //Create Square
                    GameObject squareGameObject = new GameObject();
                    Transform square = squareGameObject.transform;
                    square.parent = this.transform;
                    square.name = BoardRepresentation.SquareNameFromCoordinate(file, rank);
                    square.localScale = new Vector3(1, 1, 1) * scaleFactor;
                    square.position = PositionFromCoord(file, 0, rank);

                    // Attach a SpriteRenderer component to the GameObject
                    SpriteRenderer spriteRenderer = squareGameObject.AddComponent<SpriteRenderer>();

                    // Set the sprite for the SpriteRenderer component
                    spriteRenderer.sprite = data.square;

                    squareObjects[file, rank] = square.gameObject;

                    //Creat level Move avatar
                    GameObject legalMoveObject = Instantiate(legalMovePrefab) as GameObject;
                    Transform legalMove = legalMoveObject.transform;
                    legalMove.position= squareGameObject.transform.position;
                    legalMove.SetParent(legalMoveContainer.transform, false);
                    legalMoveObjects[file, rank]=legalMoveObject;


                    // Create piece object for current square
                    GameObject pieceObject = new GameObject("Piece");
                    pieceObject.transform.parent = square;
                    pieceObject.transform.position = PositionFromCoord(file, 0, rank);
                    squarePieceObjects[file, rank] = pieceObject;
                }
            }

            //Center the currentBoard
            //transform.position = new Vector3(-3.5f, -3.5f, 0) * scaleFactor;

            ResetSquareColours();
        }

        void ResetSquarePositions()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    if (file == 0 && rank == 0)
                    {
                        //Debug.Log (squarePieceRenderers[file, rank].gameObject.name + "  " + PositionFromCoord (file, rank, pieceDepth));
                    }
                    //squarePieceRenderers[file, rank].transform.position = PositionFromCoord (file, rank, pieceDepth);
                    squareObjects[file, rank].transform.position = PositionFromCoord(file, 0, rank);
                    squarePieceObjects[file, rank].transform.position = PositionFromCoord(file, pieceDepth, rank);
                }
            }

            if (!lastMadeMove.IsInvalid)
            {
                HighlightMove(lastMadeMove);
            }
        }

        void TurnOffAllLegalMove()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    legalMoveObjects[file, rank].SetActive(false);
                }
            }
        }


        public void SetPerspective(bool whitePOV)
        {
            whiteIsBottom = whitePOV;
            ResetSquarePositions();

        }

        public void ResetSquareColours(bool wantSetColorLastMove = true)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    SetSquareColour(new Coord(file, rank), boardTheme.lightSquares.normal, boardTheme.darkSquares.normal);
                }
            }

            TurnOffAllLegalMove();

            if (wantSetColorLastMove)
            {
                if (!lastMadeMove.IsInvalid)
                {
                    HighlightMove(lastMadeMove);
                }
            }
        }


        void SetSquareColour(Coord square, Color lightCol, Color darkCol)
        {
            squareObjects[square.fileIndex, square.rankIndex].GetComponent<SpriteRenderer>().color = (square.IsLightSquare()) ? lightCol : darkCol;
        }

        public Vector3 PositionFromCoord(int file, float depth, int rank)
        {
            if (whiteIsBottom)
            {
                return new Vector3(file, rank, depth) * scaleFactor;
            }
            return new Vector3(7 - file, 7 - rank, depth * scaleFactor);
        }

        public Vector3 PositionFromCoord(Coord coord, float depth)
        {
            return PositionFromCoord(coord.fileIndex, depth, coord.rankIndex);
        }

    }
}

