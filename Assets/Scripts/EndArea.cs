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
        GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
    }

    public override void OnReset()
    {

        GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
        
        GetComponent<CircleCollider2D>().enabled = true;

    }
}
