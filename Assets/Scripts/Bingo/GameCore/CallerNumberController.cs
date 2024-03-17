using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public class CallerNumberController : MonoBehaviour
    {
        public static CallerNumberController instance;

        public event Action<int> CallNumber;


        public int[] numbers;

        public float timeInterval = 2f;
        public float originalTimeSpeed;
        public float multiCoefficient = 1f;
        public float counterTime = 2f;

        public float readyTimeForCall = 2f;

        public int counterIndex = 0;

        public bool onCalling;


        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion
            // instantiate
            onCalling = false;
            multiCoefficient = 1f;
            counterTime = 0f;

            numbers = new int[75];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i + 1;
            }



        }
        public void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;
            BingoGameController.instance.OnEndGameEvent += OnEndGame;

            RandomFunction.SuffleAllArr(numbers);

            StartCoroutine(WaitBeforeReadyCallNumber());
        }
        private void Update()
        {
            if (onCalling)
            {
                counterTime -= Time.deltaTime * multiCoefficient;
                if (counterTime <= 0)
                {
                    SpawnNumber();
                    counterTime = timeInterval;
                }
            }


        }

        public void SpawnNumber()
        {
            if (counterIndex < numbers.Length)
            {
                CallNumber?.Invoke(numbers[counterIndex]);
                counterIndex++;
            }
        }

        public void ChangeSpeed(int _speed)
        {
            multiCoefficient = originalTimeSpeed * (_speed + 1) / 2;
        }

        // Loading 2s before call number
        public IEnumerator WaitBeforeReadyCallNumber()
        {
            yield return new WaitForSeconds(readyTimeForCall);
            onCalling = true;
        }

        #region NEW GAME , END GAME

        public void OnNewGame()
        {
            // suffle all value for new game
            RandomFunction.SuffleAllArr(numbers);

            counterIndex = 0;
            counterTime = 0f;
            onCalling = false;
            StartCoroutine(WaitBeforeReadyCallNumber());
        }
        public void OnEndGame()
        {
            onCalling = false;
        }

        public void TurnOffCalling()
        {
            onCalling = false;
        }
        public void TurnOnCalling()
        {
            onCalling = true;
        }
        #endregion

    }
}

