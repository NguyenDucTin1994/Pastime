using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess
{
    public enum Result { Playing, WhiteIsMated, BlackIsMated, Stalemate, Repetition, FiftyMoveRule, InsufficientMaterial };
    public class GameManager : MonoBehaviour
    {
       

        public  event System.Action onPositionLoaded;

        public static event Action<Vector3, int> onCaptureEvent;
        public static event Action OnNewGameEvent;
        public static event Action<Result> OnEndgameEvent;
        public static event Action<PieceColor> NextMoveEvent;
        

        public enum PlayerType { Human, AI }

        public bool isTestGame;
        public string testPosition = "r3k2r/8/8/8/8/8/3P4/R3K2R w KQkq - 0 1";

        public PlayerType whitePlayerType;
        public PlayerType blackPlayerType;
        public AISettings aiSettings;

        public TMPro.TMP_Text resultUI;

        [SerializeField]
        Result gameResult;

        Player whitePlayer;
        Player blackPlayer;
        Player playerToMove;
        List<Move> gameMoves;
        BoardController boardController;

        public float delayAnimTime = 1.25f;

        public ulong zobristDebug;
        public Board board { get; private set; }
        Board searchBoard; // Duplicate version of currentBoard used for ai search

        void Start()
        {
            testPosition = "r3k2r/8/8/8/8/8/3P4/R3K2R w KQkq - 0 1";
            //Application.targetFrameRate = 60;

            delayAnimTime = 1f;
            boardController = FindObjectOfType<BoardController>();
            gameMoves = new List<Move>();
            board = new Board();
            searchBoard = new Board();
            aiSettings.diagnostics = new Search.SearchDiagnostics();

            CanvasManager.instance.TurnOnCanvas(CanvasType.Difficulty);
            
            NewGame(whitePlayerType, blackPlayerType);
           
        }

        void Update()
        {
            zobristDebug = board.ZobristKey;

            if (gameResult == Result.Playing)
            {

                playerToMove.Update();

            }

           

        }

        void OnDoMove(Move move)
        {
           

            bool animateMove = playerToMove is AIPlayer;

            

            if (animateMove)
            {
                 delayAnimTime = UnityEngine.Random.Range(0.5f, 1.5f);
                StartCoroutine(DelayMethod(delayAnimTime, move, animateMove));
               
            }

            else
            {
                Capture(move);

                board.DoMove(move);

                searchBoard.DoMove(move);

                gameMoves.Add(move);


                boardController.OnDoMove(board, move, animateMove);

                StartCoroutine(WaitAnim());
            }

            if (animateMove)
            {
                NextMoveEvent?.Invoke(PieceColor.White);
            }
            else
            {
                NextMoveEvent?.Invoke(PieceColor.Black);
            }

        }

        IEnumerator WaitAnim()
        {
            yield return new WaitForSeconds(boardController.moveAnimDuration);
            NotifyPlayerToMove();
        }

        IEnumerator DelayMethod(float delay, Move move , bool animateMove)  // delay AI move for anim
        {
            yield return new WaitForSeconds(delay);
            Capture(move);

            board.DoMove(move);

            searchBoard.DoMove(move);

            gameMoves.Add(move);


            boardController.OnDoMove(board, move, animateMove);

            StartCoroutine(WaitAnim());

            #region Finish AI Move

            playerToMove.MakeMoveSound(true);
            if(playerToMove is AIPlayer) 
                PieceCapture.instance.UpdateNotifyNextTurn(PieceColor.Black);

            #endregion

        }
        public void NewGame(bool humanPlaysWhite)
        {
            boardController.SetPerspective(humanPlaysWhite);
            NewGame((humanPlaysWhite) ? PlayerType.Human : PlayerType.AI, (humanPlaysWhite) ? PlayerType.AI : PlayerType.Human);
            
            boardController.ResetSquareColours(false);
        }

        public void NewComputerVersusComputerGame()
        {
            boardController.SetPerspective(true);
            NewGame(PlayerType.AI, PlayerType.AI);
        }

        void NewGame(PlayerType whitePlayerType, PlayerType blackPlayerType)
        {
            gameMoves.Clear(); 

            if (isTestGame)
            {
                board.LoadPosition(testPosition);
                searchBoard.LoadPosition(testPosition);
            }

            else
            {
               
                board.LoadStartPosition();
                searchBoard.LoadStartPosition();
            }

            

            onPositionLoaded?.Invoke();
            boardController.UpdatePosition(board, true);

            boardController.ResetSquareColours(false);

            OnNewGameEvent?.Invoke();

            CreatePlayer(ref whitePlayer, whitePlayerType);
            CreatePlayer(ref blackPlayer, blackPlayerType);

            gameResult = Result.Playing;

            NotifyPlayerToMove();

        }

      

        public void QuitGame()
        {
            Application.Quit();
        }

        void NotifyPlayerToMove()
        {
            gameResult = GetGameState();

            if (gameResult == Result.Playing)
            {
                playerToMove = (board.WhiteToMove) ? whitePlayer : blackPlayer;
                playerToMove.FinishThisTurn();

            }
            else
            {
                #region END GAME

                OnEndGame(gameResult);

                #endregion

            }
        }

       

        Result GetGameState()
        {
            MoveGenerator moveGenerator = new MoveGenerator();
            var moves = moveGenerator.GenerateMoves(board);

            // Look for mate/stalemate
            if (moves.Count == 0)
            {
                if (moveGenerator.InCheck())
                {
                    return (board.WhiteToMove) ? Result.WhiteIsMated : Result.BlackIsMated;
                }
                return Result.Stalemate;
            }

            // Fifty move rule
            if (board.fiftyMoveCounter >= 100)
            {
                return Result.FiftyMoveRule;
            }

            // Threefold repetition
            int repCount = board.RepetitionPositionHistory.Count((x => x == board.ZobristKey));
            if (repCount == 3)
            {
                return Result.Repetition;
            }

            // Look for insufficient material (not all cases implemented yet)
            int numPawns = board.pawns[Board.WhiteIndex].Count + board.pawns[Board.BlackIndex].Count;
            int numRooks = board.rooks[Board.WhiteIndex].Count + board.rooks[Board.BlackIndex].Count;
            int numQueens = board.queens[Board.WhiteIndex].Count + board.queens[Board.BlackIndex].Count;
            int numKnights = board.knights[Board.WhiteIndex].Count + board.knights[Board.BlackIndex].Count;
            int numBishops = board.bishops[Board.WhiteIndex].Count + board.bishops[Board.BlackIndex].Count;

            if (numPawns + numRooks + numQueens == 0)
            {
                if ((numKnights <= 1 && numBishops==0) || (numBishops <= 1 && numKnights == 0))
                {
                    return Result.InsufficientMaterial;
                }
            }

            return Result.Playing;
        }

        void CreatePlayer(ref Player player, PlayerType playerType)
        {
            if (player != null)
            {
                player.onDoMove -= OnDoMove;
            }

            if (playerType == PlayerType.Human)
            {
                player = new HumanPlayer(board);
            }
            else
            {
                player = new AIPlayer(searchBoard, aiSettings);
            }
            player.onDoMove += OnDoMove;
        }


        public void ShowSquare()
        {
            for(int i=0; i<board.Square.Length; i++)
            {
                ChessLibrary.DebugBinaryInt(board.Square[i]);   
            }
        }

        #region FOR PLAYER PROMOTION

        public void ChoosePromotionMove(int value)
        {
           whitePlayer.ChoosePromotionMove(value);
        }

        public void DoPromotionMove()
        {
            whitePlayer.DoPromotionMove();
        }

        #endregion

        #region FOR CAPTURE PIECE

        public void Capture(Move move)
        {

            if (Piece.GetPieceTypeBinary(board.Square[move.TargetSquare]) != 0)
            {
                int captureSquare = move.TargetSquare;
                Vector2Int coord = MathFunction.ConvertListIndexToMatrixIndex(captureSquare, 8);

                Vector3 capturePos = boardController.squareObjects[coord.y, coord.x].transform.position;

                int piece = board.Square[move.TargetSquare];


                onCaptureEvent?.Invoke(capturePos, piece);
            }
        }

        #endregion

        #region DIFFICULTY + NewGame

        public void SetDifficulty(AISettings _aiSettings)
        {
            aiSettings= _aiSettings;
            NewGame(whitePlayerType, blackPlayerType);
            CanvasManager.instance.TurnOffCanvas(CanvasType.Difficulty);
        }

        public void NewMatch()
        {
            NewGame(whitePlayerType, blackPlayerType);
        }

        #endregion

        #region END GAME

        void OnEndGame(Result gameResult)
        {
            OnEndgameEvent?.Invoke(gameResult);

            CanvasManager.instance.TurnOnCanvas(CanvasType.EndGame);
        }


        #endregion

        #region FOR TEST GAME

        public void TestGame()
        {
            isTestGame = true;
            NewGame(whitePlayerType, blackPlayerType);
            CanvasManager.instance.TurnOffCanvas(CanvasType.Difficulty);
        }

        #endregion
    }


}
