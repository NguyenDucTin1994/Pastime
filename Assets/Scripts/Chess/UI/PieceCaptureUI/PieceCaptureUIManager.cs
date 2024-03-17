using Chess;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chess
{
    public class PieceCaptureUIManager : MonoBehaviour
    {
        public ChessData data;

        public PieceColor color;

        public PieceCaptureUIController[] piecesCaptureUI;

        public Image playerImage;

        public Color onTurnColor = Color.white;

        private void Awake()
        {
            piecesCaptureUI = GetComponentsInChildren<PieceCaptureUIController>();
            Array.Sort(piecesCaptureUI, new PieceUIComparer());

            SetUpPieceCaptureUI();
        }

        private void Start()
        {
            GameManager.OnNewGameEvent += OnNewGame;
        }

        public void SetUpPieceCaptureUI()
        {
            var pieceSprite = new PieceSprite();
            pieceSprite=(this.color==PieceColor.White)? data.whitePieceSprite : data.blackPieceSprite;

            for(int i = 0; i < piecesCaptureUI.Length; i++)
            {
                piecesCaptureUI[i].transform.GetComponent<Image>().sprite=pieceSprite.GetPieceSprite(i);
            }
        }

        public void OnNewGame()
        {
            for (int i = 0; i < piecesCaptureUI.Length; i++)
            {
                piecesCaptureUI[i].ReSetNumPiece();
            }
            TogglePlayerImage(color == PieceColor.Black);
        }

        public void TogglePlayerImage(bool onMove)
        {
            playerImage.color = onMove ? onTurnColor : Color.white;
        }
    }
}

