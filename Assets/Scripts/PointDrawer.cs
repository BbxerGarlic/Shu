using System;
using System.Collections.Generic;
using UnityEngine;

public class PointDrawer : MonoBehaviour, IDrawer
{
    public Pen lineMover; // 引用LineMover
    public GameObject circle;
    public GameObject lineContainer;
    private Vector3 lastPosition;
    public float minScale=0.01f;
    public float maxScale=0.5f;
    private bool started;
    
    private void Start()
    {
        lineMover = GetComponent<Pen>();
        
        
        if (Settings.isFeiBai)
        {
            enabled = false;
        }
        else
        {
            lineMover.drawer = this;
            

        }

    }

    public GameObject StartDrawing(Vector3 startPosition)
    {
        started = true;
        lastPosition = lineMover.transform.position;
        
        lineContainer = new GameObject("Line Container");
        return lineContainer;
    }

    public void EndDrawing()
    {
        started = false;
    }

    private void Update()
    {
        if(!started)return;
        
        
        Vector3 currentPosition = lineMover.transform.position;
        float distance = Vector3.Distance(lastPosition, currentPosition);

        // 插值生成点的数量，步长越小，生成的点越多
        float stepSize = 0.01f;  // 可以根据需要调整步长
        int steps = Mathf.CeilToInt(distance / stepSize);

        if (steps == 0)
        {
            var obj = Instantiate(circle, currentPosition, Quaternion.identity, lineContainer.transform);
            //obj.transform.localScale = Vector3.one * (Mathf.Clamp(Mathf.Pow(1-lineMover.GetInkRate(),0.5f)*maxScale-0.05f, minScale, maxScale) * Settings.width);
            obj.transform.localScale = Vector3.one * (Mathf.Lerp( minScale, maxScale,Mathf.Pow(1/lineMover.GetInkRate(),2f)) * Settings.width);
        }
        else
        {
            for (int i = 0; i <= steps; i++)
            {
                Vector3 position = Vector3.Lerp(lastPosition, currentPosition, i / (float)steps);
                var obj = Instantiate(circle, position, Quaternion.identity,lineContainer.transform);
                obj.transform.localScale = Vector3.one * (Mathf.Lerp( minScale, maxScale,Mathf.Pow(1/lineMover.GetInkRate(),2f)) * Settings.width);
            }
        }

        

        lastPosition = currentPosition;
        
    }


    public void UpdateDrawing(Vector3 position)
    {


    }


}


public interface IDrawer
{
    public GameObject StartDrawing(Vector3 startPosition);
    public void EndDrawing();

    public void UpdateDrawing(Vector3 position);
}