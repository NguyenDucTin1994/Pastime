using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chess
{
    public class PieceCaptureUIController : MonoBehaviour
    {
        
        public PieceType pieceType;
        public Image avatar;
        public TextMeshProUGUI numberPieceText;
        public int numberPiece=0;

        private void Awake()
        {
            avatar = GetComponent<Image>();
            numberPieceText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();    
        }

        public void IncreaseNumPiece()
        {
            numberPiece++;
            numberPieceText.text = numberPiece + "";
        }

        public void ReSetNumPiece()
        {
            numberPiece=0;
            numberPieceText.text = numberPiece + "";
        }
    }


    public class PieceUIComparer : IComparer<PieceCaptureUIController>
    {
        public int Compare(PieceCaptureUIController x, PieceCaptureUIController y)
        {
            return x.pieceType.CompareTo(y.pieceType);  
        }
    }
}

