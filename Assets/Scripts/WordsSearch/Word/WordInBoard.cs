using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WordsSearch
{
    public enum WordDirection
    {
        South, East, SouthEast , NorthEast 
    }
    public class WordInBoard 
    {

        public string word;
        public int length;
        public Vector2Int startPos;
        public Vector2Int endPos;
        public WordDirection wordDirection;
        public char[] letters;

        public WordInBoard(string word,Vector2Int startPos, WordDirection wordDirection)
        {
            this.word = word;
            this.startPos = startPos;
            this.wordDirection = wordDirection;

            length = word.Length;
            letters = word.ToCharArray();

            endPos = startPos+ (length-1)* GetValueOfDicrection(wordDirection);
        }

        public void ShowDebug()
        {
            if(this.word != null)
            {
                Debug.Log(this.word + "" + this.startPos + this.wordDirection);
            }
            
        }
        public static Vector2Int GetValueOfDicrection(WordDirection _wordDirection)
        {
            switch(_wordDirection)
            {
                case WordDirection.South:
                    return new Vector2Int(0, -1);
                case WordDirection.East:
                    return new Vector2Int(1, 0);
                case WordDirection.SouthEast:
                    return new Vector2Int(1, -1);
                default:
                    return new Vector2Int(1, 1);
            }
        }
    }


}

