using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour
{
    public Button feibaiBtn;
    public Button normalBtn;
    public Slider widthSlider;
    public GameObject point;

    private void Start()
    {
        feibaiBtn.onClick.AddListener(OnClickFeibai);
        normalBtn.onClick.AddListener(OnClickNormal);
        widthSlider.onValueChanged.AddListener(OnValueChangedWidth);
    }

    private void OnClickFeibai()
    {
        Settings.isFeiBai = true;
    }


    private void OnClickNormal()
    {
        Settings.isFeiBai = false;
    }


    private void OnValueChangedWidth(float value)
    {
        Settings.width = value;
        point.transform.localScale=Vector3.one*value*0.3f;
    }
}
