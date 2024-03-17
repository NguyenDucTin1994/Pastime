using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
    public class BoardUIManager : MonoBehaviour
    {
        private BoardUI[] boardUIs;
        private BoardUI currentBoardUI;
        public BoardUI CurrentBoardUI => currentBoardUI;

        private void Awake()
        {
            boardUIs=GetComponentsInChildren<BoardUI>();
        }

        public void TurnOnCurrentBoardUI(BoardType boardType)
        {
            currentBoardUI= boardUIs[(int) boardType];
            for(int i = 0; i < boardUIs.Length; i++)
            {
                if(i==(int) boardType)
                    boardUIs[i].gameObject.SetActive(true);
                else
                    boardUIs[i].gameObject.SetActive(false);
            }
        }
    }
}

