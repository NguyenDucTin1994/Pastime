using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class AllNumber_CellUI : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI text;
        public Image tickImage;


        private void Awake()
        {
            image = GetComponent<Image>();
            text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tickImage = transform.GetChild(0).GetComponent<Image>();
            tickImage.enabled = false;
        }

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

        }


        public void OnNewGame()
        {
            tickImage.enabled = false;
        }
    }
}

