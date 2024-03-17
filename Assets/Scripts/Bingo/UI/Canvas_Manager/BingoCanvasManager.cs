using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public class BingoCanvasManager : MonoBehaviour
    {
        [SerializeField] RectTransform bingoButtonArea;
        [SerializeField] RectTransform callerArea;
        [SerializeField] BingoGrid bingoGrid;

        private void Start()
        {
            //bingoButtonArea.sizeDelta = new Vector2(0, Screen.height/2- bingoGrid.bottomHeightOfGrid);
            //callerArea.sizeDelta = new Vector2(0, Screen.height / 2 - bingoGrid.topHeightOfGrid);
        }


    }
}

