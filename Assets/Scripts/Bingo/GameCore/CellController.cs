using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bingo
{
    public class CellController : MonoBehaviour
    {
        Button button;
        Cell cell;

        private void Awake()
        {
            button = GetComponent<Button>();
        }


    }
}

