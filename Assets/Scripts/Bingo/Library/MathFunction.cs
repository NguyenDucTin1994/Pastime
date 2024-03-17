using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathFunction 
{
    public static int ConvertMatrixIndexToListIndex(Vector2Int _pos, int _size)
    {
        return _pos.x * _size+ _pos.y;
    }

    public static Vector2Int ConvertListIndexToMatrixIndex(int index, int _size)
    {
        int y = index%_size;
        int x = (index-y)/_size;
        return new Vector2Int(x, y);
    }

    public static int MoveNextInArray(int _length,  int _index)
    {
        return (_index+1)%_length;
    }

    public static int MovePreviousInArray(int _length, int _index)
    {
        return (_index + _length - 1) % _length;
    }
}
