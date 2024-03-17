using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class ClockCounterController : MonoBehaviour
    {
        public Image clockCounter;
        public Image clockCounterBackground;

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += ClockwiseCounter;

            ClockwiseCounter();
        }
        public void ClockwiseCounter()
        {
            clockCounterBackground.enabled = true;
            clockCounter.enabled = true;
            LeanTween.value(gameObject, updateValueExampleCallback, 0f, 1f, CallerNumberController.instance.readyTimeForCall).setEase(LeanTweenType.linear)
                            .setOnComplete(OnCompleteDelay);
        }
        void updateValueExampleCallback(float val)
        {
            clockCounter.fillAmount = val;
        }

        void OnCompleteDelay()
        {
            clockCounterBackground.enabled = false;
            clockCounter.enabled = false;
        }
    }
}

