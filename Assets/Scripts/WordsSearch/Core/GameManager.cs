using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WordsSearch
{
    public class GameManager : MonoBehaviour
    {
        public static event Action<string[]> NewGameEvent;
        public static event Action EndGameEvent;

        private WordData wordData;

        public Dictionary<string, int> wordSearchDiction;

        private string[] currentWords;

        public string[] CurrenWords => currentWords;

        private void Awake()
        {
            wordData = new WordData(Path.Combine("WordsSearch", "Dictionary"));
            currentWords = wordData.wordsToPlay;
            wordSearchDiction = new Dictionary<string, int>();
        }

        private void Start()
        {
            StartCoroutine(BeginANewGame());
        }

        public void NewGame()
        {
            SetNewWordContent();

            NewGameEvent?.Invoke(CurrenWords);
        }

        public void SetNewWordContent()
        {
            wordData.ResetContent();
            currentWords = wordData.wordsToPlay;

            wordSearchDiction.Clear();
            for(int i=0; i<currentWords.Length; i++)
            {
                wordSearchDiction.Add(currentWords[i], i);
            }
        }
        public void EndGame()
        {
            CanvasManager.instance.TurnOnCanvas(CanvasType.EndGame);
            EndGameEvent?.Invoke();
        }

        public IEnumerator BeginANewGame()
        {
            yield return new WaitForEndOfFrame();
            CanvasManager.instance.TurnOnCanvas(CanvasType.Level);
        }
    }
}

