using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class EndGameUI : MonoBehaviour
    {
        public Image[] getPrizes;
        public int counter = 0;
        void OnEnable()
        {
            if (counter > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    getPrizes[i].enabled = PlayerPrizeManager.instance.isGetPrizes[i];
                }
            }
            counter++;
        }

    }
}

