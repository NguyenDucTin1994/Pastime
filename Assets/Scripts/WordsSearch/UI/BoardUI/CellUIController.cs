using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WordsSearch

{
    public class CellUIController : MonoBehaviour
    {
        public TextMeshProUGUI textMesh;
        private void Awake()
        {
            textMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }
}

