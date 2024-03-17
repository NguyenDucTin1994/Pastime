using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleList : MonoBehaviour
{
    List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

    void Suffle<T> (List<T> inputList)
    {
        for(int i=0; i<inputList.Count; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i,inputList.Count);
            inputList[i] = temp;
            inputList[rand] = temp;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("BeforeSuffle =>" + string.Join(", ", list));
            Suffle(list);
            Debug.Log("BeforeSuffle =>" + string.Join(", ", list));
        }
    }
}
