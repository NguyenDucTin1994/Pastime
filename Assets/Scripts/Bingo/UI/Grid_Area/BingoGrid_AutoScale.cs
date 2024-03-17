using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public class BingoGrid_AutoScale : MonoBehaviour
    {

        public BingoData data;
        public GameObject cellPrefab;

        private List<GameObject> cellsGameObject = new List<GameObject>();

        public CellUIController[] cellUIs;

        private void Awake()
        {

            if (cellPrefab.GetComponent<CellUIController>() == null)
            {
                Debug.LogError("this prefab need CellController script");
            }

            CreateGrid();


            cellUIs = new CellUIController[25];
            for (int i = 0; i < 25; i++)
            {
                cellUIs[i] = cellsGameObject[i].GetComponent<CellUIController>();
            }

            //Special for center cell
            //cellUIs[12].SpecialSetupCenterCell(data.centerStar);

        }

        private void Start()
        {
            BingoGameController.instance.OnNewGameEvent += OnNewGame;

            cellUIs[12].SpecialSetupCenterCell(data.centerStar);
        }

        public void CreateGrid()
        {
            SpawnGridCells();
        }
        public void SpawnGridCells()
        {
            for (int i = 0; i < 25; i++)
            {
                cellsGameObject.Add(Instantiate(cellPrefab) as GameObject);
                cellsGameObject[cellsGameObject.Count - 1].transform.SetParent(this.transform);

                // use this statement to fix in Iphone6 scale =1,7???
                cellsGameObject[cellsGameObject.Count - 1].transform.localScale = Vector3.one;
            }


        }

        public void OnNewGame()
        {
            //Special for center cell
            cellUIs[12].SpecialSetupCenterCell(data.centerStar);
        }

    }
}

