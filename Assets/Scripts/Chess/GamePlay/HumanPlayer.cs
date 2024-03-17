using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class HumanPlayer : Player
    {

        public enum InputState
        {
            None,
            PieceSelected,
            DraggingPiece
        }

        InputState currentState;

        BoardController boardController;
        Camera cam;
        Coord selectedPieceCoord;
        Board board;

        private Move chosenMove;
        private bool isFinishChooseMove;
        private bool isPromotionCurrentTurn;
        private List<Move> promotionMoves;
        private int promotionCount; // count for promotionMove(giới hạn tìm 4 nước phong : hậu xe tượng mã)
        public HumanPlayer(Board board)
        {
            boardController = Object.FindObjectOfType<BoardController>();
            cam = Camera.main;
            this.board = board;


            promotionMoves = new List<Move>();
            isPromotionCurrentTurn = false;
            isFinishChooseMove = false;
            promotionCount = 0;
        }

        public override void FinishThisTurn()
        {

        }

        public override void Update()
        {
            HandleInput();
        }

        void HandleInput()
        {
            RaycastHit2D hit;
            Vector2 mousePos, squarePos;
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);


            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                squarePos = hit.transform.position;

                if (currentState == InputState.None)
                {
                    SelectPiece(squarePos);
                }
                else if (currentState == InputState.DraggingPiece)
                {
                    DragPiece(mousePos, squarePos);
                }
                else if (currentState == InputState.PieceSelected)
                {
                    HandlePointAndClickMovement(squarePos);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CancelPieceSelection();
                }

            }
        }

        void HandlePointAndClickMovement(Vector3 mousePos)
        {
            if (Input.GetMouseButton(0))
            {
                SetPieceToTarget(mousePos);
            }
        }

        void DragPiece(Vector3 mousePos, Vector3 squarePos)
        {
            boardController.DragPiece(selectedPieceCoord, mousePos);
            // If mouse is released, then try place the piece
            if (Input.GetMouseButtonUp(0))
            {
                SetPieceToTarget(squarePos);
            }
        }

        void SetPieceToTarget(Vector3 screenPosOfTarget)
        {
            Coord targetCoord;
            if (boardController.TryGetValidCoord(screenPosOfTarget, out targetCoord)) // always change targetCoord but all method return true or false
            {
                if (targetCoord.Equals(selectedPieceCoord)) // if move to no change square , return state None
                {
                    boardController.ResetPiecePosition(selectedPieceCoord);
                    if (currentState == InputState.DraggingPiece)
                    {
                        currentState = InputState.PieceSelected;
                    }
                    else
                    {
                        currentState = InputState.None;
                        boardController.DeselectSquare(selectedPieceCoord);
                    }
                }
                else
                {
                    int targetIndex = BoardRepresentation.IndexFromCoord(targetCoord.fileIndex, targetCoord.rankIndex);
                    if (Piece.CompareColor(board.Square[targetIndex], board.ColourToMove) && board.Square[targetIndex] != 0)
                    {
                        CancelPieceSelection();
                        SelectPiece(screenPosOfTarget);
                    }
                    else
                    {
                        boardController.ResetPiecePosition(selectedPieceCoord);
                        ChooseMove(selectedPieceCoord, targetCoord);

                        if (isFinishChooseMove)
                        {
                            isFinishChooseMove = false;
                            if (isPromotionCurrentTurn)
                            {
                                isPromotionCurrentTurn = false;
                                CanvasManager.instance.TurnOnCanvas(CanvasType.Promotion);
                            }
                            else
                            {
                                DoMove(chosenMove);
                                #region Finish human turn
                                PieceCapture.instance.UpdateNotifyNextTurn(PieceColor.White);
                                SoundManager.instance.PlaySFXSound(SFXSound.Move);
                                #endregion

                            }
                           
                        }
                    }
                }
            }
            else
            {
                CancelPieceSelection();
            }

        }

        void CancelPieceSelection()
        {
            if (currentState != InputState.None)
            {
                currentState = InputState.None;
                boardController.DeselectSquare(selectedPieceCoord);
                boardController.ResetPiecePosition(selectedPieceCoord);
            }
        }

        void ChooseMove(Coord startSquare, Coord targetSquare)
        {
            int startIndex = BoardRepresentation.IndexFromCoord(startSquare);
            int targetIndex = BoardRepresentation.IndexFromCoord(targetSquare);
            
            promotionMoves.Clear();

            MoveGenerator moveGenerator = new MoveGenerator();
            bool wantsKnightPromotion = Input.GetKey(KeyCode.LeftAlt);

            var legalMoves = moveGenerator.GenerateMoves(board);

            for (int i = 0; i < legalMoves.Count; i++)
            {
                var legalMove = legalMoves[i];

                if (legalMove.StartSquare == startIndex && legalMove.TargetSquare == targetIndex)
                {
                    if(legalMove.IsPromotion)
                    {
                        isFinishChooseMove=true;
                        isPromotionCurrentTurn = true;
                        promotionMoves.Add(legalMove);
                        promotionCount++;
                        if(promotionCount > 3)
                        {
                            promotionCount = 0;
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        isFinishChooseMove = true;
                        chosenMove = legalMove;
                        break;
                    }
                }
                   

            }

            if (isFinishChooseMove)
            {
               
                currentState = InputState.None;
            }
            else
            {
                CancelPieceSelection();
            }
        }

        void SelectPiece(Vector3 mousePos)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (boardController.TryGetValidCoord(mousePos, out selectedPieceCoord))
                {
                    int index = BoardRepresentation.IndexFromCoord(selectedPieceCoord);

                    // If square contains a piece, select that piece for dragging
                    if (Piece.CompareColor(board.Square[index], board.ColourToMove))
                    {
                        boardController.SetColorValidTargetOfMove(board, selectedPieceCoord);
                        boardController.SelectSquare(selectedPieceCoord);
                        currentState = InputState.DraggingPiece;
                    }
                }
            }
        }

       
        public override void ChoosePromotionMove(int value) // 0=Queen , 1= Rook, .. theo thứ tự add trong MoveGenerator
        {
            chosenMove= promotionMoves[value];
        }

        public override void DoPromotionMove()
        {
            DoMove(chosenMove);
        }
    }
}
