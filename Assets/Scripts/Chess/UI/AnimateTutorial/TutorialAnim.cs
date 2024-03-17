using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chess

{
    public class TutorialAnim : MonoBehaviour
    {
        [SerializeField] private float duration = 3f;
        [SerializeField] private Color blackColor;
        [SerializeField] private Color whiteColor;

        [Header("Knight Piece")]
        [SerializeField] private Transform knightPiece;
        [SerializeField] Transform startPoint;
        [SerializeField] Color startColor;
        [SerializeField] Transform endPoint;
        [SerializeField] Color endColor;

        [Header("Highlight Legal Move")]
        [SerializeField] Transform[] legalMoves;
        
        public AnimationCurve animateCurve;

        private Vector3 delta;

        private void Start()
        {
            delta= endPoint.position - startPoint.position;
        }

        private void OnEnable()
        {
            ReSetAnimation();
            StartCoroutine(AnimateTutorial());
        }

        private void OnDisable()
        {
            //ReSetAnimation();
        }

        public void ReSetAnimation()
        {
            ToggleLegalMove(false);
            startPoint.GetComponent<Image>().color = blackColor;
            endPoint.GetComponent<Image>().color = whiteColor;
            knightPiece.position = startPoint.position;
        }

        public IEnumerator AnimateTutorial()
        {
            yield return new WaitForSeconds(0.6f);


            LeanTween.value(this.gameObject, UpdateValueCallBackForMovePiece, 0f, 1f, duration).setEase(animateCurve).setLoopClamp();

            LeanTween.value(this.gameObject, UpdateValueCallBackForHightLightLegalMove, 0f, 1f, duration).setEase(animateCurve).setLoopClamp();
        }

        void UpdateValueCallBackForMovePiece(float val)
        {
            if (val <= 0f)
            {
                knightPiece.gameObject.SetActive(true);
            }
            else if (val <= 1f)
            {
                knightPiece.position = startPoint.position + delta * val;
            }
            else if (val <= 1.19f)
            {
                knightPiece.gameObject.SetActive(true);
            }
            else
            {
                knightPiece.position=startPoint.position;
                knightPiece.gameObject.SetActive(true);
            }

        }

        void UpdateValueCallBackForHightLightLegalMove(float val)
        {
            if(val <= 0f)
            {
                startPoint.gameObject.GetComponent<Image>().color = startColor;
                ToggleLegalMove(false);
            }
            else if(val <= 1f)
            {
                ToggleLegalMove(true);
            }
            else if (val <= 1.19f)
            {
                ToggleLegalMove(false);
                endPoint.gameObject.GetComponent<Image>().color = endColor;
            }
            else
            {
                startPoint.GetComponent<Image>().color = blackColor;
                endPoint.GetComponent<Image>().color = whiteColor;
            }
        }

        private void ToggleLegalMove(bool isOn)
        {
            foreach(var move in legalMoves)
            {
                move.gameObject.SetActive(isOn);
            }
            
        }
    }
}