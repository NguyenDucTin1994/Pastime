using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class CellUIController : MonoBehaviour
    {
        public Toggle toggle;
        public Image image;
        public TextMeshProUGUI text;
        public Image tickImage;


        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            image = GetComponent<Image>();
            text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tickImage = transform.GetChild(0).GetComponent<Image>();
            tickImage.enabled = false;
        }

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            toggle.onValueChanged.AddListener(TickNumber);
        }

        public void TickNumber(bool onToggle)
        {
            tickImage.enabled = onToggle;
            if(onToggle)
                SoundManager.instance.PlaySFXSound(SFXSound.Move);
        }

        public void SpecialSetupCenterCell(Sprite _sprite)
        {
            StartCoroutine(SetUpCenterCell(_sprite));
        }

        public IEnumerator SetUpCenterCell(Sprite _sprite)
        {
            yield return new WaitForEndOfFrame();
            tickImage.sprite = _sprite;
            toggle.enabled = false;
            tickImage.enabled = true;
            tickImage.color = Color.white;
        }
        public void OnNewGame()
        {
            tickImage.enabled = false;
            toggle.isOn = false;
            image.color = Color.white;
        }
    }
}
