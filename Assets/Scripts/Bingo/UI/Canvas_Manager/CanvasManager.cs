using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bingo
{
    public class CanvasManager : MonoBehaviour
    {
        public static CanvasManager instance;
        public Canvas canvas;
        public List<CanvasController> canvasControllerList;
        public CanvasController lastActiveCanvas;
        public CanvasType startCanvasType;
        private void Awake()
        {
            #region SINGLE TON
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            #endregion

            canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();

            canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }

        public void Start()
        {
            canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
            SwitchCanvas(startCanvasType);
        }
        public void SwitchCanvas(CanvasType _type)
        {
            if (lastActiveCanvas != null)
            {
                lastActiveCanvas.gameObject.SetActive(false);
            }

            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            }
            else { Debug.LogWarning("The desired canvas was not found!"); }
        }

        public void SwitchCanvas(string _type)
        {
            CanvasType canvasType = (CanvasType)Enum.Parse(typeof(CanvasType), _type);

            SwitchCanvas(canvasType);
        }

        public void TurnOnCanvas(CanvasType _type)
        {
            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);

            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
            }
            else { Debug.LogWarning("The desired canvas was not found!"); }
        }
        public void TurnOnCanvas(string _type)
        {
            CanvasType canvasType = (CanvasType)Enum.Parse(typeof(CanvasType), _type);
            TurnOnCanvas(canvasType);
        }

        public void TurnOffCanvas(CanvasType _type)
        {
            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);

            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(false);
            }
            else { Debug.LogWarning("The desired canvas was not found!"); }
        }
        public void TurnOffCanvas(string _type)
        {
            CanvasType canvasType = (CanvasType)Enum.Parse(typeof(CanvasType), _type);
            TurnOffCanvas(canvasType);

        }

        public void ToggleAllNumberCanvas(bool _toggle)
        {
            CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == CanvasType.AllNumber);
            desiredCanvas.gameObject.SetActive(_toggle);
        }

        public void TurnOnCanvasInDuration(CanvasType _type, float _duration)
        {
            StartCoroutine(TurnOnCanvasInDurationCouroutine(_type, _duration));
        }

        public IEnumerator TurnOnCanvasInDurationCouroutine(CanvasType _type, float _duration)
        {
            TurnOnCanvas(_type);
            yield return new WaitForSeconds(_duration);
            TurnOffCanvas(_type);
        }
    }
}

