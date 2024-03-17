using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordsSearch
{
    public class WordData
    {
        private string[] topics;
        public string[] wordsToPlay;
        public WordData(string filePath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(filePath);

            // Get an array of lines from the text asset, each line is a topic
            topics = textAsset.text.Split('\n');


            wordsToPlay = new string[9];

            ResetContent();
        }

        public void ResetContent()
        {
            int numberOfWordsMin = 0;
            while(numberOfWordsMin<9)
            {
                // Choose a random topic
                string randomTopic = topics[UnityEngine.Random.Range(0, topics.Length)];

                // Split the line into an array of wordsInRandomTopic
                string[] wordsInRandomTopic = randomTopic.Trim().Split(',');

                numberOfWordsMin = wordsInRandomTopic.Length;

                if(numberOfWordsMin >= 9 ) 
                {
                    RandomFunction.ShufflePartOfArr(wordsInRandomTopic, 9);

                    for (int i = 0; i < 9; i++)
                    {
                        wordsToPlay[i] = wordsInRandomTopic[i];
                    }
                }
                
            }
            

            //wordsToPlay = wordsToPlay.OrderBy(x =>100- x.Length).ToArray();
        }
    }
}

