using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Texture[] textures;

    public int fps=30;
    public float fpsCounter=0f;
    public int currentIndex = 0;



    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        fpsCounter+= Time.deltaTime;    
        if(fpsCounter > 1f/fps)
        {
            currentIndex++;
            if(currentIndex >= textures.Length)
            {
                currentIndex = 0;
            }
            lineRenderer.material.SetTexture("_MainTex", textures[currentIndex]);
            fpsCounter = 0;

        }
    }
}
