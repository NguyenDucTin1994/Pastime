using Chess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordsSearch
{
    public class AdjustRectTransInWorld : MonoBehaviour
    {
        private int size;
        private RectTransform rectTransform;

        public GridLayoutGroup gridLayout;

        private float standardSize = 8f;
        private Vector3 defaulCamPos = new Vector3((8f / 2f - 0.5f), (8f / 2 - 0.5f), 0);

        private void Awake()
        {
            size =8+(int)GetComponent<BoardUI>().boardType;
            gridLayout = GetComponent<GridLayoutGroup>();
            gridLayout.constraintCount = size;
            rectTransform = GetComponent<RectTransform>();

        }

        private void Start()
        {
            AdjustSquareRecTrans(standardSize);
            SetUpGridLayout();
            Vector3 delta = Camera.main.transform.position - defaulCamPos- new Vector3(1,1,0) * (1-8/(float)size)*0.5f;
            MoveRectTrans(delta);
        }
        public void AdjustSquareRecTrans(float _standardSize)
        {
            Vector2 worldSize = rectTransform.TransformVector(rectTransform.rect.size);


            Vector2 pixelSize = rectTransform.rect.size;
            Vector2 cellSize = gridLayout.cellSize;


            rectTransform.sizeDelta = new Vector2(pixelSize.x, pixelSize.y) * _standardSize / worldSize.x;


            gridLayout.cellSize = cellSize * _standardSize / worldSize.x;
            //Debug.Log("gridLayout.cellSize" + gridLayout.cellSize);
        }

        public void MoveRectTrans(Vector3 delta)
        {
            rectTransform.position -= delta;
        }

        public void SetUpGridLayout()
        {
            gridLayout.constraintCount = this.size;
            gridLayout.cellSize = gridLayout.cellSize * 8f /this.size;
            //Debug.Log("gridLayout.cellSize" + gridLayout.cellSize);
        }
    }
}

