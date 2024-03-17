using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Chess
{
    public static class ChessLibrary 
    {
        public static void DebugBinaryInt(int _value)
        {
            string binaryString = Convert.ToString(_value, 2);
            Debug.Log(binaryString);
        }

        public static void DebugStatic(int _value)
        {
            Debug.Log(_value);
        }
    }
}

