using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bingo
{
    public class BingoController : MonoBehaviour
    {
        public Button bingoClick;
        public TextMeshProUGUI bingoText;
        public Image bingoImage;

        public bool canClick = true;
        public float timePunishment = 0f;
        public int numberOfBingoFailContinuous;

        [SerializeField]
        private BoardPlayer player;


        public void Awake()
        {

            bingoClick = GetComponent<Button>();
            bingoClick.onClick.AddListener(OnBingoButonClick);

            bingoText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            bingoImage = transform.GetChild(1).GetComponent<Image>();

            canClick = true;
            timePunishment = 0f;
            numberOfBingoFailContinuous = 0;

        }

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;
        }

        private void Update()
        {
            if (timePunishment > 0f)
            {
                bingoImage.enabled = false;
                canClick = false;
                timePunishment -= Time.deltaTime;
                bingoText.text = Mathf.CeilToInt(timePunishment).ToString();
            }
            else
            {
                bingoImage.enabled = true;
                canClick = true;
                bingoText.text = "BINGO";
            }
        }
        public void OnBingoButonClick()
        {
            if (canClick)
            {
                player.BingoClick();

                if (player.bingoSuccess)
                {
                    timePunishment = 0f;
                    numberOfBingoFailContinuous = 0;
                }
                else
                {
                    numberOfBingoFailContinuous++;
                    timePunishment = 3f * numberOfBingoFailContinuous;
                }
            }
        }

        public void OnNewGame()
        {
            timePunishment = 0;
            numberOfBingoFailContinuous = 0;
        }
    }
}

