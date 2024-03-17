using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    [CreateAssetMenu(menuName = "Scriptable Object/Chess/Board")]
    public class BoardTheme : ScriptableObject
    {
        public SquareColours lightSquares;
        public SquareColours darkSquares;

        [System.Serializable]
        public struct SquareColours
        {
            public Color normal;
            public Color check;
            public Color selected;
            public Color moveFromHighlight;
            public Color moveToHighlight;
        }
    }
}

