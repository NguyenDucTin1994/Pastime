using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordsSearch
{
    public class WordTextManager : MonoBehaviour
    {
        public WordText[] wordTexts;
        private void Awake()
        {
            wordTexts=GetComponentsInChildren<WordText>();
        }

        private void Start()
        {
            GameManager.NewGameEvent += OnNewGame;
            DrawLineController.DetectWordEvent += OnDectectWord;
        }

        public void OnNewGame(string[] wordsContent)
        {
            for (int i=0; i<wordTexts.Length; i++)
            {
                wordTexts[i].textMesh.text = wordsContent[i];
                wordTexts[i].spikeThrough.enabled = false;
            }
        }

        public void OnDectectWord(int _wordIndex)
        {
            wordTexts[_wordIndex].UpdateFindedWord();
        }
    }
}

