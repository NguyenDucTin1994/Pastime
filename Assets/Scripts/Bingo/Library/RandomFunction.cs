using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RandomFunction
{
    public static void Suffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    // get 5 number random in list
   public static void ShuffleList<T>(List<T> inputList, int randoms)
    {
        for (int i = 0; i < randoms; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    public static void SuffleAllArr<T>(T[] inputArr)
    {
        for (int i = 0; i < inputArr.Length; i++)
        {
            T temp = inputArr[i];
            int rand = Random.Range(i, inputArr.Length);
            inputArr[i] = inputArr[rand];
            inputArr[rand] = temp;
        }
    }

    // get 5 number random in Array
    public static void ShufflePartOfArr<T>(T[] inputArr, int randoms)
    {
        for (int i = 0; i < randoms; i++)
        {
            T temp = inputArr[i];
            int rand = Random.Range(i, inputArr.Length);
            inputArr[i] = inputArr[rand];
            inputArr[rand] = temp;
        }
    }
}
