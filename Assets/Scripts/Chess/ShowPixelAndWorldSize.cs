using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPixelAndWorldSize : MonoBehaviour
{
    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void ShowPixelSizeDebug()
    {
        Vector2 worldSize = rectTransform.TransformVector(rectTransform.rect.size);
        Vector2 pixelSize = rectTransform.rect.size;
        Debug.Log("World Size: " + worldSize.ToString("F2")  );
        Debug.Log("Pixel Size: " + pixelSize.ToString("F2"));
        Debug.Log("World Pos: " + transform.position.ToString("F2"));


    }
}
