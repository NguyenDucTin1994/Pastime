using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

namespace Bingo
{
    public class BallController : MonoBehaviour
    {

        public TextMeshProUGUI numberText;
        public Image backGroundImage;
        public ColumnType columnType;


        public int index;

        public IBallState currentState;
        public IBallState ballCallingState;
        public IBallState ballRecentCalledState;
        public IBallState beforeWaiting;
        public IBallState ballWaitingState;

        public float timeInterval;

        private void Awake()
        {
            numberText = GetComponentInChildren<TextMeshProUGUI>();
            backGroundImage = GetComponentInChildren<Image>();
            timeInterval = 0.75f;

            ballCallingState = new BallCallingState(this);
            ballRecentCalledState = new BallRecentCalledState(this);
            beforeWaiting = new BallBeforeWaitingState(this);
            ballWaitingState = new BallWaitingState(this);

            UpdateStateOfBall();
        }
        void Start()
        {

        }

        public void Move(Vector3[] positions)
        {


            currentState.Move(positions);
        }

        public void SetUp(int _callingNumber)
        {
            numberText.text = _callingNumber.ToString();

            if (_callingNumber > 0 && _callingNumber <= 15)
            {
                columnType = ColumnType.B;
            }
            else if (_callingNumber > 15 && _callingNumber <= 30)
            {
                columnType = ColumnType.I;
            }
            else if (_callingNumber > 30 && _callingNumber <= 45)
            {
                columnType = ColumnType.N;
            }
            else if (_callingNumber > 45 && _callingNumber <= 60)
            {
                columnType = ColumnType.G;
            }
            else if (_callingNumber > 60 && _callingNumber <= 75)
            {
                columnType = ColumnType.O;
            }
        }

        public void UpdateStateOfBall()
        {
            switch (index)
            {
                case 0:
                    currentState = ballCallingState;
                    break;
                case 5:
                    currentState = ballWaitingState;
                    break;
                case 4:
                    currentState = beforeWaiting;
                    break;
                default:
                    currentState = ballRecentCalledState;
                    break;

            }
        }

        #region LeanTween 

        public void LeanTweenMove(Vector3 _target)
        {
            LeanTween.move(this.gameObject, _target, timeInterval).setEase(LeanTweenType.easeOutQuad);
        }

        public void LeanTweenMoveEaseOutBounce(Vector3 _target)
        {
            LeanTween.move(this.gameObject, _target, timeInterval).setEase(LeanTweenType.easeOutBounce);
        }

        public void LeanTweenRotateAround()
        {
            LeanTween.rotateAround(this.gameObject, Vector3.forward, -360f, timeInterval).setEase(LeanTweenType.easeOutQuad);
        }

        public void LeanTweenScale(float _scale)
        {
            LeanTween.scale(this.gameObject, Vector3.one * _scale, timeInterval).setEase(LeanTweenType.linear);
        }

        #endregion

    }

    public class BallComparer : IComparer<BallController>
    {
        public int Compare(BallController x, BallController y)
        {
            return x.index.CompareTo(y.index);
        }
    }

    #region STATE PATTERN

    public interface IBallState
    {

        public void Move(Vector3[] positions);

    }

    public class BallCallingState : IBallState
    {
        private readonly BallController ballController;

        public BallCallingState(BallController _ballController)
        {
            this.ballController = _ballController;
        }

        public void Move(Vector3[] positions)
        {
            ballController.LeanTweenMove(positions[MathFunction.MoveNextInArray(6, ballController.index)]);
            ballController.LeanTweenScale(1f);

        }

    }

    public class BallRecentCalledState : IBallState
    {
        private readonly BallController ballController;

        public BallRecentCalledState(BallController _ballController)
        {
            this.ballController = _ballController;
        }
        public void Move(Vector3[] positions)
        {
            ballController.LeanTweenMove(positions[MathFunction.MoveNextInArray(6, ballController.index)]);
            ballController.LeanTweenRotateAround();
        }

    }

    public class BallBeforeWaitingState : IBallState
    {
        private readonly BallController ballController;

        public BallBeforeWaitingState(BallController _ballController)
        {
            this.ballController = _ballController;
        }
        public void Move(Vector3[] positions)
        {
            ballController.LeanTweenMove(positions[MathFunction.MoveNextInArray(6, ballController.index)]);
            ballController.LeanTweenScale(0f);
        }

    }

    public class BallWaitingState : IBallState
    {
        private readonly BallController ballController;

        public BallWaitingState(BallController _ballController)
        {
            this.ballController = _ballController;
        }

        public void Move(Vector3[] positions)
        {
            ballController.LeanTweenMoveEaseOutBounce(positions[MathFunction.MoveNextInArray(6, ballController.index)]);
            ballController.LeanTweenScale(1.5f);
        }


    }

    #endregion
}

