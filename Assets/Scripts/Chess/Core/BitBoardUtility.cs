using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////////////// ADD COMMENT BY TIN
namespace Chess
{
    public static class BitBoardUtility
    {
        // extension method for determine bit position (int) square is equal 0 or 1 => true or false
        // bit wise operator ( right-shift >>) eliminate (int) square bit from right
        // bit wise AND (&) with 1 => get the end bit (0 or 1)
        public static bool ContainsSquare(ulong bitboard, int square)
        {
            return ((bitboard >> square) & 1) != 0;
        }
    }
}