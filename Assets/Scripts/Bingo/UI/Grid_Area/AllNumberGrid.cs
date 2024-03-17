using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public class AllNumberGrid : MonoBehaviour
    {
        public int columns = 5;
        public int rows = 5;
        public float cellOffset = 0f;

        public BingoData data;
        public GameObject cellPrefab;



        public Vector2 deltaPosFromCenter; // delta between currentBoard center and center of screen
        public Vector2 startPos = new Vector2(0, 0); // position in left up corner of currentBoard

        public float cellSize;
        public float alignHorizontal;

        private List<GameObject> gridCells = new List<GameObject>();

        public CellUIController[] cellUIs;

        private void Awake()
        {



            alignHorizontal = 0.6f * Screen.width / (columns + 4f);
            cellSize = (0.6f * Screen.width - 2 * alignHorizontal) / columns;

            startPos.x = -((cellSize + cellOffset) * columns * 0.5f + deltaPosFromCenter.x);
            startPos.y = (cellSize + cellOffset) * rows * 0.5f + deltaPosFromCenter.y;

            if (cellPrefab.GetComponent<CellUIController>() == null)
            {
                Debug.LogError("this prefab need CellController script");
            }

            CreateGrid();



            cellUIs = new CellUIController[75];
            for (int i = 0; i < 75; i++)
            {
                cellUIs[i] = gridCells[i].GetComponent<CellUIController>();
            }


        }
        private void Start()
        {
            CallerNumberController.instance.CallNumber += UpdateCallingNumber;
            SetValue();
        }

        public void CreateGrid()
        {
            SpawnGridCells();
            SetCellsPosition();
        }
        public void SpawnGridCells()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    gridCells.Add(Instantiate(cellPrefab) as GameObject);
                    gridCells[gridCells.Count - 1].transform.SetParent(this.transform);

                    var cell_Rect = gridCells[gridCells.Count - 1].GetComponent<RectTransform>();
                    cell_Rect.sizeDelta = new Vector2(cellSize, cellSize);
                }

            }
        }

        public void SetCellsPosition()
        {
            var cell_Rect = gridCells[0].GetComponent<RectTransform>();

            Vector2 offset = new Vector2();
            offset.x = cellSize + cellOffset;
            offset.y = cellSize + cellOffset;

            int colum_number = 0;
            int row_number = 0;

            foreach (var _cell in gridCells)
            {
                if (colum_number + 1 > columns)
                {
                    colum_number = 0;
                    row_number++;
                }

                var pos_x_offset = offset.x * colum_number + cellSize / 2;
                var pos_y_offset = offset.y * row_number + cellSize / 2;

                _cell.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y - pos_y_offset);
                colum_number++;
            }


        }

        public void SetValue()
        {
            for (int i = 0; i < 75; i++)
            {
                Vector2Int pos = MathFunction.ConvertListIndexToMatrixIndex(i, 5);
                cellUIs[i].text.text = "" + (15 * pos.y + pos.x);
            }
        }

        public void UpdateCallingNumber(int _newNumber)
        {
            Vector2Int pos = MathFunction.ConvertListIndexToMatrixIndex(_newNumber, 15);
            int index = pos.y * 5 + pos.x;
            cellUIs[index].tickImage.enabled = true;
        }
    }
}
