using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public static class Piece
    {

        public const int None = 0;
        public const int King = 1;
        public const int Pawn = 2;
        public const int Knight = 3;
        public const int Bishop = 5;
        public const int Rook = 6;
        public const int Queen = 7;

        public const int White = 8;
        public const int Black = 16;

        const int typeMask = 0b00111;
        const int blackMask = 0b10000;
        const int whiteMask = 0b01000;
        const int colourMask = whiteMask | blackMask;

        public static bool CompareColor(int piece, int compareColor)
        {
            return (piece & colourMask) == compareColor;
        }

        public static int GetColor(int piece)
        {
            return piece & colourMask;
        }

        public static bool IsWhitePiece(int piece)
        {
            int tempColor = piece & colourMask;
            if (tempColor == White)
                return true;
            else
                return false;
        }
        public static PieceColor GetPieceColor(int piece)
        {
            int tempColor = piece & colourMask;
            if (tempColor == White)
                return PieceColor.White;
            else
                return PieceColor.Black;
        }

        public static int GetPieceTypeBinary(int piece)
        {
            return piece & typeMask;
        }

        public static PieceType GetPieceType(int piece)
        {
            int tempPieceType = piece & typeMask;
            switch (tempPieceType)
            {
                case 1:
                    return PieceType.King;
                case 2:
                    return PieceType.Pawn;
                case 3:
                    return PieceType.Knight;
                case 5:
                    return PieceType.Bishop;
                case 6:
                    return PieceType.Rook;
                case 7:
                    return PieceType.Queen;

                default:
                    return PieceType.Pawn;
            }
        }
        public static bool IsRookOrQueen(int piece)
        {
            return (piece & 0b110) == 0b110;
        }

        public static bool IsBishopOrQueen(int piece)
        {
            return (piece & 0b101) == 0b101;
        }

        public static bool IsSlidingPiece(int piece)
        {
            return (piece & 0b100) != 0;
        }
    }
}

