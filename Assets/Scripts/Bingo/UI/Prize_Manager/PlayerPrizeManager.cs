using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class PlayerPrizeManager : MonoBehaviour
    {
        public static PlayerPrizeManager instance;
        public PlayerPrizeUIController[] playerPrizesImage = new PlayerPrizeUIController[6];
        public BingoData data;

        public bool[] changedPrizesOnce; // control for check condition once time (control for check prize with more time =>fail)

        public bool[] isGetPrizes = new bool[6] { false, false, false, false, false, false };

        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion


            //
            changedPrizesOnce = new bool[6] { true, true, true, true, true, true };
        }

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            // Instatiate player prize images
            for (int i = 0; i < playerPrizesImage.Length; i++)
            {
                playerPrizesImage[i] = transform.GetChild(i).GetComponent<PlayerPrizeUIController>();
                playerPrizesImage[i].prizeImage.sprite = data.prizesConditionSprite[i];
            }
        }
        public void PlayerGetPrize(int _prize)
        {
            if (changedPrizesOnce[(int)_prize] == true)
            {
                playerPrizesImage[_prize].checkImage.sprite = data.getSprite;
                playerPrizesImage[_prize].checkImage.enabled = true;
                playerPrizesImage[_prize].prizeImage.sprite = data.prizesConditionSpriteOff[_prize];

                changedPrizesOnce[(int)_prize] = false;
                isGetPrizes[(int)_prize] = true;
            }

        }

        // when a prize has got by boss
        public void PlayerCantGetPrize(int _prize)
        {
            if (changedPrizesOnce[(int)_prize] == true)
            {
                playerPrizesImage[_prize].checkImage.sprite = data.dontGetPrize;
                playerPrizesImage[_prize].checkImage.enabled = true;
                playerPrizesImage[_prize].prizeImage.sprite = data.prizesConditionSpriteOff[_prize];

                changedPrizesOnce[(int)_prize] = false;
                isGetPrizes[(int)_prize] = false;
            }
        }

        public void OnNewGame()
        {
            // Instatiate player prize images
            for (int i = 0; i < playerPrizesImage.Length; i++)
            {
                changedPrizesOnce[i] = true; // fix for new game don't check pize
                playerPrizesImage[i].checkImage.enabled = false;
                playerPrizesImage[i].prizeImage.sprite = data.prizesConditionSprite[i];
            }

        }
    }
}

