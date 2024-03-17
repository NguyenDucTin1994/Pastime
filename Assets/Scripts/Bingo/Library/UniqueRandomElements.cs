using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueRandomElements : MonoBehaviour
{
    List<int> list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    List<T> GetUniqueRandomElements<T>(List<T> inputList, int count)
    {
        List<T> inputListClone = new List<T>(inputList);
        Shuffle(inputListClone);
        return inputListClone.GetRange(0, count);
    }

    void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var uniqueRandomList = GetUniqueRandomElements(list, 4);

            /*Debug.Log("All elements => " + string.Join(", ", list));
            Debug.Log("Unique random elements => " + string.Join(", ", uniqueRandomList));
            Debug.Log("*************************************************************");
            */
        }
    }
}
