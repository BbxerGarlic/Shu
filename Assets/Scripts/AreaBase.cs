using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AreaBase : MonoBehaviour
{
    private int round = 0;
    protected virtual void OnPenEnter()
    {

    }

    protected virtual void OnPenExit()
    {

    }

    public virtual void OnReset()
    {
        
    }


    private void Awake()
    {
        round = GetNumberFromName(transform.parent.name);
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {

    }

    protected virtual void Start()
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
        
        if (other.CompareTag("Pen"))
        {
            OnPenExit();
        }
    
    }
    
    int GetNumberFromName(string name)
    {
        // 使用正则表达式匹配末尾的数字
        Match match = Regex.Match(name, @"\d+$");
        if (match.Success)
        {
            // 将匹配到的字符串转换为整数
            return int.Parse(match.Value);
        }
        else
        {
            // 如果没有匹配到数字，返回一个默认值
            return -1;
        }
    }
    
}
