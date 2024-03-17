using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
        public enum CanvasType
        {
            GameScreen,
            Level,
            Tutorial,
            EndGame
        }
        public class CanvasController : MonoBehaviour
        {
            public CanvasType canvasType;
        }
}

