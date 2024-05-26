using System;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour,IDrawer
{
    public Pen lineMover; // 引用LineMover
    public InkState[] inkStates; // 包含所有状态的数组
    public GameObject endCapPrefab; // 停顿点的Prefab
    public int extraPoints = 5; // 在Inspector中设置的额外点数
    private int extraCount;
    private int currentPrefabIndex = 0;
    public float timeStep = 0.1f;
    private float timeSinceLastPoint = 0f;
    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private LineRenderer lastLine;
    private GameObject lineContainer;
    private List<GameObject> endCapList = new List<GameObject>(); // 存储endCap的链表
    public float scaleSpeed = 1.0f; // 变大速度
    public float maxScale = 2.0f; // 最大值
    public float endCapThreshold = 0.05f;

    private void Start()
    {
        
        lineMover = GetComponent<Pen>();
        

        
        if (!Settings.isFeiBai)
        {
            enabled = false;
        }
        else
        {
            lineMover.drawer = this;
        }
    }


    void Update()
    {
        if (lineMover.IsDrawing())
        {
            timeSinceLastPoint += Time.deltaTime;

            if (timeSinceLastPoint >= timeStep)
            {
                AddPointToLine(lineRenderer);
                if (extraCount > 0)
                {
                    AddPointToLine(lastLine);
                    extraCount--;
                }
                timeSinceLastPoint = 0f;
            }
            
            CheckInkStatusAndUpdate();
        }


    }

    public GameObject StartDrawing(Vector3 startPosition)
    {
        lineRenderer = null;
        currentLine = null;
        
        //transform.position = startPosition;
        
        endCapList.Clear();
        
        lineContainer = new GameObject("Line Container");
        Debug.Log(123);
        SpawnNewLine();
        return lineContainer;
    }

    public void EndDrawing()
    {
        if (lineMover.IsDrawing())
        {
            CreateEndCapAtPosition(transform.position);
        }
    }

    public void UpdateDrawing(Vector3 position)
    {
        if (Vector3.Distance(lineMover.transform.position, position) < GetEndCapThreshold())
        {
            if (!CheckEndCapProximityAndScale() && lineRenderer.positionCount > 4)
            {
                CreateEndCapAtPosition(lineRenderer.GetPosition(lineRenderer.positionCount - 3));
            }
        }

        //transform.position = position;
    }

    public float GetEndCapThreshold()
    {
        return endCapThreshold;
    }
    
    void CheckInkStatusAndUpdate()
    {
        float inkValue = lineMover.GetInkValue();
        for (int i = inkStates.Length - 1; i >= 0; i--)
        {
            if (inkValue > inkStates[i].inkThreshold)
            {
                if (i != currentPrefabIndex)
                {
                    currentPrefabIndex = i;
                    lastLine = lineRenderer;
                    extraCount = extraPoints;
                    SpawnNewLine();
                }
                break;
            }
        }
    }

    void AddPointToLine(LineRenderer line)
    {
        UpdateMaterialProperties(line.material, line);
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, transform.position);

    }

    void SpawnNewLine()
    {
        // if (currentLine != null)
        // {
        //     transform.position = currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1);
        // }
        Vector3 target = transform.position;

        if (currentLine != null && lineRenderer.positionCount > 5)
        {
            target = lineRenderer.GetPosition(lineRenderer.positionCount - 4);
        }
        
        currentLine = Instantiate(inkStates[currentPrefabIndex].linePrefab, transform.position, Quaternion.identity, lineContainer.transform);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, target);
        lineRenderer.widthMultiplier = Settings.width;
    }

    void ClearAllLines()
    {
        foreach (Transform child in lineContainer.transform)
        {
            Destroy(child.gameObject);
        }
        endCapList.Clear();
    }

    void CreateEndCapAtPosition(Vector3 position)
    {
        if (endCapList.Count <= 4) position = transform.position;
        GameObject newEndCap = Instantiate(endCapPrefab, position, Quaternion.identity,lineContainer.transform);
        float value = lineMover.GetInkValue() / lineMover.GetMaxInk();
        newEndCap.transform.localScale *= value;
        endCapList.Add(newEndCap);
    }

    bool CheckEndCapProximityAndScale()
    {
        
        foreach (GameObject endCap in endCapList)
        {
            float distance = Vector3.Distance(transform.position, endCap.transform.position);
            if (distance < GetEndCapThreshold())
            {
                float scaleFactor = Mathf.Min(maxScale, endCap.transform.localScale.x + scaleSpeed * Time.deltaTime * lineMover.GetInkValue() / lineMover.GetMaxInk());
                endCap.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor)*Settings.width;
                return true;
            }
        }

        return false;
    }

    public void UpdateMaterialProperties(Material material, LineRenderer lineRenderer)
    {
        float totalLength = CalculateLineLength(lineRenderer);
        float lineWidth = lineRenderer.startWidth;
        material.SetFloat("_LineWidth", lineWidth);

        float textureRepeatCount = totalLength / material.mainTexture.width;
        material.SetFloat("_TextureAmount", textureRepeatCount);
        material.SetFloat("_TextureLength", totalLength / textureRepeatCount);
    }

    private float CalculateLineLength(LineRenderer lineRenderer)
    {
        float totalLength = 0f;
        Vector3 previousPoint = lineRenderer.GetPosition(0);

        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            Vector3 currentPoint = lineRenderer.GetPosition(i);
            totalLength += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        return totalLength;
    }

    [System.Serializable]
    public class InkState
    {
        public GameObject linePrefab;
        public float inkThreshold;

        public InkState(float threshold, GameObject prefab)
        {
            inkThreshold = threshold;
            linePrefab = prefab;
        }
    }
}
