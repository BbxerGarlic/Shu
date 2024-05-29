using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndArea : AreaBase
{
    protected override void OnPenEnter()
    {
        //TODO：在这里实现效果
        GetComponent<SpriteRenderer>().DOColor(Color.green, 0.5f);

    }

    protected override void OnPenExit()
    {
        if(GetComponent<CircleCollider2D>().enabled == true)GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
    }

    public override void OnReset()
    {

        GetComponent<SpriteRenderer>().DOColor(Color.green-new Color(0,0,0,0.5f), 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = true;

    }

    public void OnEnd()
    {
        //TODO:游戏结束时调用
        GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void OnCannotEnd()
    {
        
        //TODO:条件不满足时调用
        GetComponent<SpriteRenderer>().DOColor(Color.red, 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
