using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Bingo
{
    public enum TimeSpeedType
    {
        Half,
        Normal,
        OneAndAHalf,
        Double
    }
    public class TimeSpeedChange : MonoBehaviour
    {
        public BingoData data;
        public string[] timeSpeeds;
        public TextMeshProUGUI timeSpeedText;
        public int currentIndex;

        private void Awake()
        {
            timeSpeeds = new string[] { "0.5x", "1x", "1.5x", "2x" };
            currentIndex = 1;
            timeSpeedText = transform.GetComponentInChildren<TextMeshProUGUI>();
            timeSpeedText.text = timeSpeeds[currentIndex];
        }

        public void MoveNextSpeed()
        {
            currentIndex = MathFunction.MoveNextInArray(4, currentIndex);
            timeSpeedText.text = timeSpeeds[currentIndex];
            LeanTween.scale(gameObject, Vector3.one * 1.5f, 0.2f).setEase(LeanTweenType.easeOutQuad).
                setOnComplete(() => LeanTweenScale(1f));
            CallerNumberController.instance.ChangeSpeed(currentIndex);
        }

        public void LeanTweenScale(float scale)
        {
            LeanTween.scale(gameObject, Vector3.one * scale, 0.2f).setEase(LeanTweenType.easeOutQuad);
        }
    }
}

