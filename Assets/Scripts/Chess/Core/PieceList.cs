
// COOMMENT BY TIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class PieceList  // Array of position of one GetPieceTypeBinary in the currentBoard
    {

        // Indices of squares occupied by given piece type (only elements up to Count are valid, the rest are unused/garbage)
        public int[] occupiedSquares;
        // Map to go from index of a square, to the index in the occupiedSquares array where that square is stored
        int[] map; //64 ô bàn cờ , map[34] = 1, nghĩa là ô 34 có GetPieceTypeBinary , nó nằm ở occupiedSquares[1]
        int numPieces; // numPiece là số quân cờ có mặt (0--numPiece-1) , phần tử thứ numPiece để chờ thêm vào

        public PieceList(int maxPieceCount = 16)
        {
            occupiedSquares = new int[maxPieceCount];
            map = new int[64];
            numPieces = 0;
        }

        public int Count
        {
            get
            {
                return numPieces;
            }
        }

        public void AddPieceAtSquare(int square)
        {
            occupiedSquares[numPieces] = square;
            map[square] = numPieces;
            numPieces++;
        }

        public void RemovePieceAtSquare(int square) // remove bằng cách xóa sau do chuyển phần tử có giá trị cuối trong mảng(thứ [numPiece-1] ) về vị trí xóa , giảm numPiece--)
        {
            int pieceIndex = map[square]; // get the index of this element in the occupiedSquares array

            occupiedSquares[pieceIndex] = occupiedSquares[numPieces - 1]; // move last element in array to the place of the removed element

            map[occupiedSquares[pieceIndex]] = pieceIndex; // update map to point to the moved element's new location in the array

            numPieces--;
        }

        public void MovePiece(int startSquare, int targetSquare)
        {
            int pieceIndex = map[startSquare]; // get the index of this element in the occupiedSquares array
            occupiedSquares[pieceIndex] = targetSquare;
            map[targetSquare] = pieceIndex;
        }

        public int this[int index] => occupiedSquares[index];

    }
}

