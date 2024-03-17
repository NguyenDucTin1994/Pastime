using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chess
{
    public class PieceCapture : MonoBehaviour
    {
        public static PieceCapture instance;
        [SerializeField]
        private PieceCaptureUIManager player1CaptureUI;
        [SerializeField]
        private PieceCaptureUIManager player2CaptureUI;
        private PieceCaptureUIManager currentBeCapturedPlayer;

        public SpriteRenderer moveCapturePiece;
        

        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion
            player1CaptureUI = transform.GetChild(0).GetComponentInChildren<PieceCaptureUIManager>();
            player2CaptureUI = transform.GetChild(1).GetComponentInChildren<PieceCaptureUIManager>();
        }

        private void Start()
        {
            GameManager.onCaptureEvent += OnCapture;
        }
        public void OnCapture(Vector3 capturePos, int piece)
        {
            int pieceIndex = (int)Piece.GetPieceType(piece);

            

            currentBeCapturedPlayer = (Piece.IsWhitePiece(piece)) ? player2CaptureUI : player1CaptureUI;

            PieceCaptureUIController target = currentBeCapturedPlayer.piecesCaptureUI[pieceIndex];
            moveCapturePiece.sprite = target.avatar.sprite;

            moveCapturePiece.transform.position = capturePos;
            moveCapturePiece.enabled = true;

            target.IncreaseNumPiece();

            LeanTween.move(moveCapturePiece.gameObject, target.transform.position, 0.5f).setEase(LeanTweenType.easeOutQuad).
                setOnComplete(() =>
                {
                    moveCapturePiece.enabled = false;
                    LeanTween.scale(target.gameObject, Vector3.one * 2, 0.3f).setOnComplete( () => LeanTween.scale(target.gameObject, Vector3.one * 1f, 0.3f) );
                });

        }

        public void UpdateNotifyNextTurn(PieceColor pieceColor)
        {
            player1CaptureUI.TogglePlayerImage(player1CaptureUI.color == pieceColor);
            player2CaptureUI.TogglePlayerImage(player2CaptureUI.color == pieceColor);
        }

    }
}

