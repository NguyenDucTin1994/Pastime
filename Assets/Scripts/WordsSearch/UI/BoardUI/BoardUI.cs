using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordsSearch
{
    public class BoardUI : MonoBehaviour
    {
        public BoardType boardType;

        [SerializeField] private BoardController boardController;
        [SerializeField] private GameObject cellUIPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;
        public CellUIController[] cellsUI;
        // Start is called before the first frame update
        void Start()
        {
            gridLayout = GetComponent<GridLayoutGroup>();
            CreatBoadrdUI();

        }

        public void CreatBoadrdUI()
        {
            int size = 8 + (int)boardType;
            cellsUI = new CellUIController[size*size];

            for (int i = 0; i < cellsUI.Length; i++)
            {
                GameObject cellUIObject = Instantiate(cellUIPrefab) as GameObject;

                cellUIObject.transform.SetParent(this.transform);
                cellUIObject.transform.localScale = Vector3.one;

                cellsUI[i] = cellUIObject.GetComponent<CellUIController>();
                cellsUI[i].textMesh.fontSize =108*8/size;
            }
        }

        public void UpdateBoard(Board _board)
        {
            for (int i = 0; i < cellsUI.Length; i++)
            {
                Vector2Int index2D = MathFunction.ConvertListIndexToMatrixIndex(i, boardController.Size);
                cellsUI[i].textMesh.text = _board.cells[index2D.y,index2D.x].letter.ToString();
            }
        }

    }
}

