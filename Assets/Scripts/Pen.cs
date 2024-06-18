using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pen : MonoBehaviour
{
    public float speed = 0.1f;
    [SerializeField]private float inkValue = 100.0f;
    [SerializeField]private float maxInk = 100.0f;
    [SerializeField]private float inkRechargeRate = 1.0f;
    [SerializeField]private float inkUsagePerMeter = 1.0f;

    
    
    private Vector3 targetPosition;
    private bool started = false;
    private bool ended = false;

    public IDrawer drawer;

    private Vector3 lastPos;
    public Vector3 velocity;
    
    public float currentVelocity = 0;

    private GameObject line;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            if(ended) return;

            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            if (!started)
            {
                
                RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero,Mathf.Infinity,LayerMask.GetMask("StartArea"));

                if (hit||Settings.isDuanBi&&line!=null||!Settings.isMiaoHong)
                {
                    started = true;
                    transform.position = targetPosition;
                    GetComponent<CircleCollider2D>().enabled = true;
                    lastPos = targetPosition;
                    line=drawer.StartDrawing(targetPosition);

                }
                
            }

        }

        currentVelocity = velocity.magnitude;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(ended||!started) return;

            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            MoveToPoint(targetPosition);

            velocity = (transform.position - lastPos)/Time.deltaTime;
            lastPos = transform.position;
            
            inkValue += inkRechargeRate * Time.deltaTime;
            inkValue = Mathf.Min(maxInk, inkValue);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!ended&&started)
            {
                ended = true;
                drawer.EndDrawing();

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero,Mathf.Infinity,LayerMask.GetMask("EndArea"));

                if (hit)
                {
                    if (TargetManager.Instance.EndGame())
                    {
                        Invoke(nameof(ContinueGame), 0.5f);
                        
                        hit.collider.GetComponent<EndArea>().OnEnd();
                        return;
                    }
                    else
                    {
                        hit.collider.GetComponent<EndArea>().OnCannotEnd();
                    }
                    
                }
                
                Invoke(nameof(RestartGame), 0.2f);


            }   
        }


    }

    void ContinueGame()
    {
        line = null;
        started = false;
        ended = false;
        GetComponent<CircleCollider2D>().enabled = false;

    }
    
    void RestartGame()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        if (!Settings.isDuanBi)
        {
            TargetManager.Instance.ResetGame();
            Destroy(line);
            line = null;
        }

        
        started = false;
        ended = false;

    }

    void MoveToPoint(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);
        float interpolatedSpeed = Mathf.Lerp(0.1f, 5f, distance / 10.0f) * this.speed;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, interpolatedSpeed * Time.deltaTime);
        float stepDistance = Vector3.Distance(transform.position, newPosition);

        transform.position = newPosition;
        inkValue = Mathf.Max(inkValue - stepDistance * inkUsagePerMeter,0);
        
        if(Mathf.Abs(velocity.magnitude)<=0.00002f)drawer.UpdateDrawing(transform.position);
    }

    public bool IsDrawing()
    {
        return started && !ended;
    }

    public float GetInkRate()
    {
        return Mathf.Max(0.00001f,inkValue/maxInk);
    }
    
    public float GetSpeed()
    {
        //return inkValue;
        return Mathf.Abs(velocity.magnitude);
    }
    public float GetSpeedRate()
    {
        //return Mathf.Max(0.00001f,inkValue/maxInk);

        return Mathf.Clamp(Mathf.Abs(velocity.magnitude)/speed,0.001f,99f);
    }
    public float GetMaxInk()
    {
        return maxInk;
    }

    public void SwitchPen()
    {
        if (Settings.isFeiBai)
        {
            Settings.isFeiBai = false;
            GetComponent<LineDrawer>().enabled = false;
            GetComponent<PointDrawer>().enabled = true;
            drawer = GetComponent<PointDrawer>();
        }
        else
        {
            Settings.isFeiBai = true;
            GetComponent<LineDrawer>().enabled = true;
            GetComponent<PointDrawer>().enabled = false;
            drawer = GetComponent<LineDrawer>();
        }
    }
}
