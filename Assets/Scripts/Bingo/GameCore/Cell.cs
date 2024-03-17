using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bingo
{
    public enum ColumnType
    {
        B, I, N, G, O,
    }
    public class Cell
    {
        public Vector2Int position;
        public ColumnType columnType;
        public int value;
        public bool isCalled;

    }

    public class CellComparer : IComparer<Cell>
    {
        public int Compare(Cell x, Cell y)
        {
            return x.value.CompareTo(y.value);
        }
    }

}

