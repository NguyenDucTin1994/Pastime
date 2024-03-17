using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Bingo
{
    public class BingoGameController : MonoBehaviour
    {
        public static BingoGameController instance;

        public event Action OnNewGameEvent;
        public event Action OnEndGameEvent;

        public BoardPlayer playerBoard;


        public bool[] havingPrizes;

        public bool canNewGame = true;

        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion
            havingPrizes = new bool[6] { true, true, true, true, true, true };
        }

        public void PlayerCantGetPrize() // boss winned the prize 
        {
            for (int i = 0; i < havingPrizes.Length; i++)
            {
                if (havingPrizes[i] == false)
                {
                    PlayerPrizeManager.instance.PlayerCantGetPrize(i);
                }
            }
        }

        public void PlayerGetPrize()
        {
            for (int i = 0; i < havingPrizes.Length; i++)
            {
                if (havingPrizes[i] == false && playerBoard.getPrizes[i] == true)
                    PlayerPrizeManager.instance.PlayerGetPrize(i);
            }
        }

        public void PlayerGetPrize(Prize _prize)
        {
            PlayerPrizeManager.instance.PlayerGetPrize((int)_prize);
        }

        public void OnNewGame()
        {

            if (canNewGame)
            {
                canNewGame = false;
                OnNewGameEvent?.Invoke();

                // Reset Prize
                for (int i = 0; i < havingPrizes.Length; ++i)
                {
                    havingPrizes[i] = true;
                }

                CanvasManager.instance.TurnOffCanvas(CanvasType.EndGame);

                playerBoard.archievedPrizes.Clear();
                StartCoroutine(DelayNewGame());
            }


        }


        public IEnumerator DelayNewGame()
        {
            yield return new WaitForSeconds(3f);
            canNewGame = true;
        }
        public void OnEndGame()
        {
            OnEndGameEvent?.Invoke();
            CanvasManager.instance.TurnOnCanvas(CanvasType.EndGame);
        }

    }
}
