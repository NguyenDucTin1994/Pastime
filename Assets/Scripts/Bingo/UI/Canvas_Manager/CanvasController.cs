using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public enum CanvasType
    {

        GameScreen,
        PrizeCondition,
        GetPrize,
        DontHavePrize,
        AllNumber,
        EndGame

    }
    public class CanvasController : MonoBehaviour
    {
        public CanvasType canvasType;
    }
}

