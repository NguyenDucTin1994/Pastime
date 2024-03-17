using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCam;
    public RectTransform boardBorders;
    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }
    private void Start()
    {
        // 8.410.. is the world size of the boardBorder size in reference resolution (1080x2340) , camera orthorgraphicsize =12;
        mainCam.orthographicSize = mainCam.orthographicSize * 8.41026f/boardBorders.TransformVector(boardBorders.rect.size).x ;

    }
}
