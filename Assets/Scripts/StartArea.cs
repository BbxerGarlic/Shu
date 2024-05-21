using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartArea : AreaBase
{
    protected override void OnPenEnter()
    {
        
        //TODO：在这里实现效果
        GetComponent<SpriteRenderer>().DOColor(Color.green, 0.5f);
        
    }

    protected override void OnPenExit()
    {
        GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.5f);
        GetComponent<CircleCollider2D>().enabled = false;
    }
    
    public override void OnReset()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#FFC107", out color))
        {
            
        }
        else
        {
            Debug.LogWarning("Invalid hex color string");
            color=Color.black; // 返回一个默认颜色
        }
        GetComponent<SpriteRenderer>().DOColor(color, 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = true;

    }
    
}
