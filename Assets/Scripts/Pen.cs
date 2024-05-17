using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pen : MonoBehaviour
{
    public float speed = 0.1f;
    public float inkValue = 100.0f;
    public float maxInk = 100.0f;
    public float inkRechargeRate = 1.0f;
    public float inkUsagePerMeter = 1.0f;

    private Vector3 targetPosition;
    private bool started = false;
    private bool ended = false;

    public IDrawer drawer;

    private Vector3 lastPos;
    public Vector3 velocity;

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

                if (hit)
                {
                    started = true;
                    transform.position = targetPosition;
                    drawer.StartDrawing(targetPosition);
                    lastPos = transform.position;
                }
                
            }

        }
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
                        return;
                    }
                    
                }
                
                Invoke(nameof(RestartGame), 1.0f);


            }   
        }


    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public float GetInkValue()
    {
        return inkValue;
    }
    public float GetInkRate()
    {
        return Mathf.Max(0.00001f,inkValue/maxInk);
    }
    public float GetMaxInk()
    {
        return maxInk;
    }


}
