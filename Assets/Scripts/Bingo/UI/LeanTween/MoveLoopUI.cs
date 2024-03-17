using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoopUI : MonoBehaviour
{

    public float scale = 1.5f;
    public int repeatTime=1;
    public float timeInterval = 1f;
    public bool isLoop=false;

    public Transform target;
    private void OnEnable()
    {
        isLoop = true;
        if(isLoop)
        {
            LeanTween.move(gameObject, target.position, timeInterval).setLoopPingPong();
        }
    }

    private void OnDisable()
    {
       isLoop=false;
    }
}
