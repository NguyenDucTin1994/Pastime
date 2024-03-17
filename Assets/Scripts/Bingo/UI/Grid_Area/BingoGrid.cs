using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class BingoGrid : MonoBehaviour
    {
        public int columns = 5;
        public int rows = 5;
        public float cellOffset = 0f;

        public BingoData data;
        public GameObject cellPrefab;
        public GameObject columnHeaderPrefab;
        public Sprite[] columnsHeaderSprite;

        public Vector2 deltaPosFromCenter; // delta between currentBoard center and center of screen
        public Vector2 startPos = new Vector2(0, 0); // position in left up corner of currentBoard

        public float cellSize;
        public float alignHorizontal;
        public int topSpace;
        public int bottomSpace;
        public float topHeightOfGrid { get; private set; } //distance from  center of screen to top of gid
        public float bottomHeightOfGrid { get; private set; } //distance from  center of screen to bot of gid


        private List<GameObject> gridCells = new List<GameObject>();

        public CellUIController[] cellUIs;

        private void Awake()
        {

            columnsHeaderSprite = data.columnsHeaderSprite;

            alignHorizontal = Screen.width / (columns + 4f);
            cellSize = (Screen.width - 2 * alignHorizontal) / columns;

            startPos.x = -((cellSize + cellOffset) * columns * 0.5f + deltaPosFromCenter.x);
            startPos.y = (cellSize + cellOffset) * rows * 0.5f + deltaPosFromCenter.y;

            if (cellPrefab.GetComponent<CellUIController>() == null)
            {
                Debug.LogError("this prefab need CellController script");
            }

            CreateGrid();

            //use for align other top and bot UI (??)
            topHeightOfGrid = (cellSize + cellOffset) * rows * 0.5f + deltaPosFromCenter.y + topSpace;
            bottomHeightOfGrid = (cellSize + cellOffset) * rows * 0.5f - deltaPosFromCenter.y + bottomSpace;


            cellUIs = new CellUIController[25];
            for (int i = 0; i < 25; i++)
            {
                cellUIs[i] = gridCells[i].GetComponent<CellUIController>();
            }


        }
        private void Start()
        {

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

            #region CREATE COLUMN TYPR HEADER

            for (int i = 0; i < columns; i++)
            {
                GameObject header = Instantiate(columnHeaderPrefab);
                header.transform.SetParent(this.transform);
                //header.GetComponent<ColumnHeaderController>().columnHeaderImage.sprite = columnsHeaderSprite[i];

                var header_rect = header.GetComponent<RectTransform>();
                header_rect.sizeDelta = new Vector2(cellSize, cellSize);

                var pos_x_offset = offset.x * i + cellSize / 2;
                var pos_y_offset = offset.y - cellSize / 2;
                header_rect.anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y + pos_y_offset);
            }
            #endregion

        }





    }
}

