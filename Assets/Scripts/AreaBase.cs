using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBase : MonoBehaviour
{
    
    protected virtual void OnPenEnter()
    {

    }

    protected virtual void OnPenExit()
    {

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pen"))
        {
            OnPenEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        {
            if (other.CompareTag("Pen"))
            {
                OnPenExit();
            }
        }
    }
}
