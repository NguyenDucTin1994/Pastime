using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLeanTween : MonoBehaviour
{
    public Transform target;
    public float timeInterval;
    public Image image;

    public Transform[] road;
    public LTSpline spline;
    public Vector3[] path;

    private void Start()
    {
        path = new Vector3[]
        {
            road[0].position,
            road[0].position,
            road[1].position,
            road[2].position,
            road[3].position,
            road[3].position,
        };

    }
    public void MoveLeanTween()
    {

        LeanTween.move(this.gameObject, target.position,timeInterval).setEase(LeanTweenType.easeOutQuad);
       
        LeanTween.rotateAround(this.gameObject, Vector3.forward, 360f, timeInterval).setEase(LeanTweenType.easeOutQuad);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MoveSpline();
            Debug.Log("Move");
        }
        
    }
    public void ClockwiseCounter()
    {
        LeanTween.value(gameObject, updateValueExampleCallback, 0f, 1f, 10f).setEase(LeanTweenType.linear);
       
    }
    void updateValueExampleCallback(float val)
    {
        image.fillAmount = val;
    }

    public void MoveSpline()
    {
        LeanTween.moveSpline(gameObject, path, 10f).setOrientToPath2d(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(spline!=null)
        {
            spline.gizmoDraw();
        }
    }
}
