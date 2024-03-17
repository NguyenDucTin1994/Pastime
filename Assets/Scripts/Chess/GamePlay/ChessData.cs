using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    [CreateAssetMenu(menuName = "Scriptable Object/Chess/Chess Data")]
    public class ChessData : ScriptableObject
    {
        public Sprite square;

        public PieceSprite whitePieceSprite;

        public PieceSprite blackPieceSprite;

    }

    [Serializable]
    public class PieceSprite
    {
        public Sprite king, pawn, knight, bishop, rook, queen;
        public Sprite GetPieceSprite(int pieceIndex)
        {
            switch (pieceIndex)
            {
                case 0:
                    return this.king;
                case 1:
                    return this.pawn    ;
                case 2:
                    return this.knight;
                case 3:
                    return this.bishop;
                case 4:
                    return this.rook;
                case 5:
                    return this.queen;
                default: return null;
            }
        }
    }
    public enum PieceType
    {
        King,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
    }

    public enum PieceColor
    {
        White,
        Black,
    }
}

