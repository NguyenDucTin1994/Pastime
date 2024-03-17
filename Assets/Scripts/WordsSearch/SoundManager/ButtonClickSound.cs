using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordsSearch
{
    public class ButtonClickSound : MonoBehaviour
    {
        [SerializeField] Button[] buttons;

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();
        }

        private void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(() => SoundManager.instance.PlaySFXSound(SFXSound.Click));
            }
        }
    }

}
