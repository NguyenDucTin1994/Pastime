using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordsSearch
{
    public class TutorialAnim : MonoBehaviour
    {
        [SerializeField] private float duration=3f;

        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] Transform startPoint;
        [SerializeField] Transform endPoint;
        [SerializeField] Color color;
        private Vector3 delta;

        [SerializeField] Image wordImage;

        public AnimationCurve drawLineCurve;
        public AnimationCurve activeImageCurve;
        
        private void Awake()
        {
            delta = endPoint.position - startPoint.position;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            ReSetAnimation();
        }

        private void OnEnable()
        {
            ReSetAnimation();
            StartCoroutine(AnimateTutorial());
        }

        private void OnDisable()
        {
            ReSetAnimation();
        }

        public void ReSetAnimation()
        {
            lineRenderer.widthMultiplier = 0f;
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, startPoint.position);
            
            wordImage.enabled = false;
        }

        public IEnumerator AnimateTutorial()
        {
            yield return new WaitForSeconds(0.6f);
           
            lineRenderer.widthMultiplier = 1f;

            LeanTween.value(this.gameObject, UpdateValueCallBackForDrawLine, 0f, 1f, duration).setEase(drawLineCurve).setLoopClamp();

            LeanTween.value(this.gameObject, UpdateValueCallBackForActiveIamge, 0f, 1f, duration).setEase(activeImageCurve).setLoopClamp();
        }

        void UpdateValueCallBackForDrawLine(float val)
        {
            if (val >= 1.2f)
            {
                lineRenderer.widthMultiplier = 0f;
            }
            else
            {
                lineRenderer.widthMultiplier = 1f;
                if(val <=1f)
                lineRenderer.SetPosition(1, startPoint.position + delta * val);
            }
        }

        void UpdateValueCallBackForActiveIamge(float val)
        {
            if(val >= 1 && val <1.19f)
                wordImage.enabled=true;
            else
            {
                wordImage.enabled=false;
            }
        }
    }
}

