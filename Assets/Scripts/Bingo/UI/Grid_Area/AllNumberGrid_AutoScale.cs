using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Bingo
{
    public class AllNumberGrid_AutoScale : MonoBehaviour
    {

        public BingoData data;
        public GameObject cellPrefab;

        private List<GameObject> cellsGameObject = new List<GameObject>();

        public AllNumber_CellUI[] cellUIs;

        private void Awake()
        {
            if (cellPrefab.GetComponent<AllNumber_CellUI>() == null)
            {
                Debug.LogError("this prefab need CellController script");
            }

            CreateCellsGameObject();



            cellUIs = new AllNumber_CellUI[75];

            for (int i = 0; i < 75; i++)
            {
                cellUIs[i] = cellsGameObject[i].GetComponent<AllNumber_CellUI>();
            }

            SetValue();
            CallerNumberController.instance.CallNumber += UpdateCallingNumber;

        }
        private void Start()
        {

        }

        public void CreateCellsGameObject()
        {
            cellsGameObject = new List<GameObject>();
            for (int i = 0; i < 75; i++)
            {
                cellsGameObject.Add(Instantiate(cellPrefab) as GameObject);
                cellsGameObject[cellsGameObject.Count - 1].transform.SetParent(this.transform);

                // use this statement to fix in Iphone6 scale =1,7???
                cellsGameObject[cellsGameObject.Count - 1].transform.localScale = Vector3.one;
            }
        }


        public void SetValue()
        {
            for (int i = 0; i < 75; i++)
            {
                Vector2Int pos = MathFunction.ConvertListIndexToMatrixIndex(i, 5);
                cellUIs[i].text.text = "" + (15 * pos.y + pos.x + 1);
            }
        }

        public void UpdateCallingNumber(int _newNumber)
        {
            Vector2Int pos = MathFunction.ConvertListIndexToMatrixIndex(_newNumber - 1, 15);
            int index = pos.y * 5 + pos.x;
            cellUIs[index].tickImage.enabled = true;
        }
    }
}

