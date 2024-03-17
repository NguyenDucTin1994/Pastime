using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public enum CanvasType
    {
        GameScreen,
        Difficulty,
        Tutorial,
        Promotion,
        EndGame


    }
    public class CanvasController : MonoBehaviour
    {
        public CanvasType canvasType;
    }
}

