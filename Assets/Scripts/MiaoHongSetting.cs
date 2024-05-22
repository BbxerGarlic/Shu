using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiaoHongSetting : MonoBehaviour
{
    public bool isFeiBai;
    public float brushWidth=0.5f;

    private void Awake()
    {
        Settings.isFeiBai = isFeiBai;
        Settings.width = brushWidth;
    }
}
