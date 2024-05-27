using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Target : AreaBase
{

    public Vector2 VelocityDir=>transform.up;
    public float angle;
    [FormerlySerializedAs("inkMaxMinValue")] [FormerlySerializedAs("velocityMaxMinValue")] [Tooltip("墨水区间，x为墨水最大值，y为墨水最小值")]public Vector2 inkMinMaxValue;

    private Pen pen;


    protected override void Start()
    {
        base.Start();
        pen = GameObject.FindWithTag("Pen").GetComponent<Pen>();


    }

    private void OnTargetDone()
    {
        TargetManager.Instance.RemoveTarget(this);

        GetComponent<CircleCollider2D>().enabled = false;
        
        
        
        //TODO：在这里实现效果
        GetComponent<SpriteRenderer>().DOColor(UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f), 0.5f);
        transform.DOScale(Vector3.one * 0.4f, 0.5f);



    }
    
    public override void OnReset()
    {
        //TODO: 这里可以实现重置效果
        GetComponent<SpriteRenderer>().DOColor(Color.red, 0.5f);
        transform.DOScale(Vector3.one * 0.2f, 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = true;
    }

    protected override void OnPenEnter()
    {
        

        Vector2 penVelocity = pen.velocity;

        // 计算速度值
        float inkValue = pen.GetInkValue();

        // 检查速度是否在指定范围内
        if (inkValue >= inkMinMaxValue.x && inkValue <= inkMinMaxValue.y)
        {
//            Debug.Log(123);
            // 归一化向量
            Vector2 normalizedPenVelocity = penVelocity.normalized;
            Vector2 normalizedVelocityDir = VelocityDir.normalized;

            // 计算点积
            float dotProduct = Vector2.Dot(normalizedPenVelocity, normalizedVelocityDir);

            // 计算夹角（弧度）
            float angleBetween = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

            // 检查夹角是否小于指定角度
            if (angleBetween < angle)
            {
                // 满足条件，执行你的逻辑
                Debug.Log("Pen velocity direction and speed meet the requirements.");
                OnTargetDone();
            }
        }
    }
    
    protected override void OnPenExit()
    {
        
    }


}
