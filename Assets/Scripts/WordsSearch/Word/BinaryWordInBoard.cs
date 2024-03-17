using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
    public static class BinaryWordInBoard 
    {

        const int START_POS = 0b11111111; //8 last bit for start pos (0-63)
        public static int GetStartPos(int binaryWordInBoard)
        {
                return binaryWordInBoard & START_POS;
        }


        const int WORD_DIRECTION = 0b1100000000; //2 bit  for word direction (4 value S,N,SW,NW)
        public static int GetWordDirection(int binaryWordInBoard)
        {
            return (binaryWordInBoard & WORD_DIRECTION) >> 8;
        }


        const int COUNT_DUPLICATE_CHAR = 0b11110000000000; //chars in word in currentBoard duplicate 


        public static WordInBoard GetWordInBoard(string word, int binaryWordValue, int size)
        {
            int tempt = GetStartPos(binaryWordValue);
            Vector2Int temptVector2Int = MathFunction.ConvertListIndexToMatrixIndex(tempt, size);
            Vector2Int startPos = new Vector2Int(temptVector2Int.x, temptVector2Int.y);

            WordDirection direction = (WordDirection)GetWordDirection(binaryWordValue);

            return new WordInBoard(word, startPos, direction);


        }


        public static int GetCompareValue(int binaryWordInBoard)
        {
            return (binaryWordInBoard & COUNT_DUPLICATE_CHAR) >> 10 ;
        }

        public static int MakeBinaryWordValue(int _startPos, int _wordDirec,   int _countDuplicateChar)
        {
            return _startPos | _wordDirec << 8 | _countDuplicateChar <<10 ;
        }

    }

    public class BinaryWordInBoardComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return BinaryWordInBoard.GetCompareValue(x).CompareTo(BinaryWordInBoard.GetCompareValue(y));
        }
    }
}

