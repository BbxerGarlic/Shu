using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    public InkState[] inkStates; // 包含所有状态的数组
    public GameObject endCapPrefab; // 停顿点的Prefab
    public int extraPoints = 5; // 在Inspector中设置的额外点数
    private int extraCount;
    private int currentPrefabIndex = 0;
    public float timeStep = 0.1f;
    public float speed = 0.1f;
    private float timeSinceLastPoint = 0f;
    private Vector3 targetPosition;
    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private LineRenderer lastLine;
    private GameObject lineContainer;
    private GameObject endCap;
    public float inkValue = 100.0f;
    public float maxInk = 100.0f;
    public float inkRechargeRate = 1.0f;
    public float inkUsagePerMeter = 1.0f;
    
    public float endCapThreshold = 0.05f; // 划线停顿的最小移动距离阈值
    private List<GameObject> endCapList = new List<GameObject>(); // 存储endCap的链表
    public float scaleSpeed = 1.0f; // 变大速度
    public float maxScale = 2.0f; // 最大值
    bool started = false;
    bool ended = false;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(ended)return;

            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            if (!started)
            {
                started = true;
                transform.position = targetPosition;
                
                lineContainer = new GameObject("Line Container");
                SpawnNewLine();
            }
            
            MoveToPoint(targetPosition);
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
            
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!ended)
            {
                ended = true;
            }   
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAllLines();
        }
        
        inkValue += inkRechargeRate * Time.deltaTime;
        inkValue = Mathf.Min(maxInk, inkValue);

        if(started&&!ended)CheckInkStatusAndUpdate();

    }

    void MoveToPoint(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);
        float interpolatedSpeed = Mathf.Lerp(0.1f, 5f, distance / 10.0f) * this.speed;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, interpolatedSpeed * Time.deltaTime);
        float stepDistance = Vector3.Distance(transform.position, newPosition);

        if (stepDistance < endCapThreshold)
        {
            //CreateEndCapAtPosition(transform.position);
            if (!CheckEndCapProximityAndScale())
            {
                
                if(started&&!ended&&lineRenderer.positionCount>4)CreateEndCapAtPosition(lineRenderer.GetPosition(lineRenderer.positionCount-3));
            };
        }
        
        transform.position = newPosition;
        inkValue = Mathf.Clamp(inkValue - stepDistance * inkUsagePerMeter, 0, maxInk);
    }

    void CheckInkStatusAndUpdate()
    {
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
        UpdateMaterialProperties(line.material,line);
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, transform.position);
    }

    void SpawnNewLine()
    {
        if (currentLine != null)
        {
            transform.position = currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1);
        }

        currentLine = Instantiate(inkStates[currentPrefabIndex].linePrefab, transform.position, Quaternion.identity, lineContainer.transform);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        
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
        GameObject newEndCap = Instantiate(endCapPrefab, position, Quaternion.identity);
        float value = inkValue / maxInk;
        newEndCap.transform.localScale *=value;
        endCapList.Add(newEndCap);
    }
    
    bool CheckEndCapProximityAndScale()
    {
        foreach (GameObject endCap in endCapList)
        {
            float distance = Vector3.Distance(transform.position, endCap.transform.position);
            if (distance < endCapThreshold)
            {
                float scaleFactor = Mathf.Min(maxScale, endCap.transform.localScale.x + scaleSpeed * Time.deltaTime*inkValue/maxInk);
                endCap.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                return true;
            }
        }

        return false;
    }

    public void UpdateMaterialProperties(Material material, LineRenderer lineRenderer)
    {
        Debug.Log(lineRenderer.name);
        // 计算 LineRenderer 的总长度
        float totalLength = CalculateLineLength(lineRenderer);

        // 设置材质的线宽属性
        float lineWidth = lineRenderer.startWidth;  // 假设线宽是均匀的
        material.SetFloat("_LineWidth", lineWidth);

        float textureRepeatCount = totalLength / material.mainTexture.width;
        material.SetFloat("_TextureAmount", textureRepeatCount);
        
        material.SetFloat("_TextureLength", totalLength/textureRepeatCount);
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
        public GameObject linePrefab; // 对应的线条Prefab
        public float inkThreshold; // 这个Prefab所需要的墨水阈值

        public InkState(float threshold, GameObject prefab)
        {
            inkThreshold = threshold;
            linePrefab = prefab;
        }
    }
}
