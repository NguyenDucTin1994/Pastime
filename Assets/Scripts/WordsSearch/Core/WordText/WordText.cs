using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WordsSearch
{
    public class WordText : MonoBehaviour
    {
        public Image spikeThrough;
        public TextMeshProUGUI textMesh;

        private void Awake()
        {
            spikeThrough=GetComponent<Image>();
            
            textMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public void UpdateFindedWord()
        {
            spikeThrough.enabled = true;
        }

    }
}

