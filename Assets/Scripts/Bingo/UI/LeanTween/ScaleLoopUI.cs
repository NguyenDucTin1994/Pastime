using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLoopUI : MonoBehaviour
{

    public float scale = 1.5f;
    public int repeatTime = 1;
    public float timeInterval = 1f;
    private LTDescr scaleTween;

    private void OnEnable()
    {
        scaleTween = LeanTween.scale(gameObject, Vector3.one * scale, timeInterval).setLoopPingPong();
    }

    private void OnDisable()
    {
        if (scaleTween != null)
        {
            LeanTween.cancel(scaleTween.id);
            scaleTween = null;
        }
        transform.localScale = Vector3.one;
    }
}
