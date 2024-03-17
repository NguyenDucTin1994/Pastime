using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class ColumnHeaderController : MonoBehaviour
    {
        public BingoData data;
        public GameObject headerPrefab;

        private List<GameObject> headersGameObject = new List<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < 5; i++)
            {
                headersGameObject.Add(Instantiate(headerPrefab) as GameObject);
                headersGameObject[headersGameObject.Count - 1].transform.SetParent(this.transform);
                headersGameObject[headersGameObject.Count - 1].GetComponent<Image>().sprite = data.columnsHeaderSprite[i];
            }
        }

    }
}

