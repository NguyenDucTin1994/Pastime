using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class PlayerPrizeUIController : MonoBehaviour
    {
        public Image prizeImage;
        public Image checkImage;

        private void Awake()
        {
            prizeImage = GetComponent<Image>();
            checkImage = transform.GetChild(0).GetComponent<Image>();

            checkImage.enabled = false;
        }

    }
}

