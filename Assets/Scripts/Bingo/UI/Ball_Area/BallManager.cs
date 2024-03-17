using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bingo
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager instance;
        public BingoData data;

        [SerializeField] BallController[] balls;
        public Vector3[] ballsPosition;
        public Transform startSpawnPosition;

        public int waitingBallPosition;

        private void Awake()
        {
            #region SINGLETON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion
        }
        void Start()
        {
            CallerNumberController.instance.CallNumber += UpdateUIOnCallingNewNumber;
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            balls = GetComponentsInChildren<BallController>();
            Array.Sort(balls, new BallComparer());
            waitingBallPosition = balls.Length - 1;

            ballsPosition = new Vector3[balls.Length];
            ballsPosition = balls.Select(x => x.transform.position).ToArray();

            // set active fall for all ball
            for (int i = 0; i < balls.Length; i++)
                balls[i].gameObject.SetActive(false);

        }

        public void UpdateUIOnCallingNewNumber(int _callingNumber)
        {
            StartCoroutine(UpdateBalls(_callingNumber));
        }
        public IEnumerator UpdateBalls(int _callingNumber)
        {
            yield return new WaitForSeconds(0.3f);

            //Ball only visible when be called (when start it is set active false)
            balls[waitingBallPosition].gameObject.SetActive(true);

            balls[waitingBallPosition].SetUp(_callingNumber);
            balls[waitingBallPosition].backGroundImage.sprite = data.ballSprites[(int)(balls[waitingBallPosition].columnType)];


            balls[waitingBallPosition].transform.position = startSpawnPosition.position;
            waitingBallPosition = MathFunction.MovePreviousInArray(balls.Length, waitingBallPosition);


            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].Move(ballsPosition);
                balls[i].index = MathFunction.MoveNextInArray(6, balls[i].index);
                balls[i].UpdateStateOfBall();
            }
        }

        public void OnNewGame()
        {
            Array.Sort(balls, new BallComparer());
            waitingBallPosition = balls.Length - 1;

            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = ballsPosition[i];
                balls[i].gameObject.SetActive(false);
            }

        }

    }
}

