using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    [CreateAssetMenu (menuName ="Scriptable Object/Chess/Piece")]
    public class PieceTheme : ScriptableObject
    {
        public PiecePrefabs whitePieces;
        public PiecePrefabs blackPieces;

        public GameObject GetPiecePrefab(int piece)
        {
            PiecePrefabs piecePrefabs = Piece.CompareColor(piece, Piece.White) ? whitePieces : blackPieces;

            switch (Piece.GetPieceTypeBinary(piece))
            {
                case Piece.Pawn:
                    return piecePrefabs.pawn;
                case Piece.Rook:
                    return piecePrefabs.rook;
                case Piece.Knight:
                    return piecePrefabs.knight;
                case Piece.Bishop:
                    return piecePrefabs.bishop;
                case Piece.Queen:
                    return piecePrefabs.queen;
                case Piece.King:
                    return piecePrefabs.king;
                default:
                    if (piece != 0)
                    {
                        Debug.Log(piece);
                    }
                    return null;
            }
        }

        [System.Serializable]
        public class PiecePrefabs
        {
            public GameObject pawn, rook, knight, bishop, queen, king;

            public GameObject this[int i]
            {
                get
                {
                    return new GameObject[] { pawn, rook, knight, bishop, queen, king }[i];
                }
            }
        }
    }
}

