using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour
{
    public Button feibaiBtn;
    public Button normalBtn;
    public Slider widthSlider;
    public Toggle duanBiToggle;
    public GameObject point;

    private void Start()
    {
        feibaiBtn.onClick.AddListener(OnClickFeibai);
        normalBtn.onClick.AddListener(OnClickNormal);
        widthSlider.onValueChanged.AddListener(OnValueChangedWidth);
        duanBiToggle.onValueChanged.AddListener(OnValueChangedDuanBi);
        duanBiToggle.isOn = Settings.isDuanBi;
    }

    private void OnValueChangedDuanBi(bool arg0)
    {
        Settings.isDuanBi = arg0;
        
    }

    private void OnClickFeibai()
    {
        Settings.isFeiBai = true;
        SceneManager.LoadScene("CreateLevel1");
    }


    private void OnClickNormal()
    {
        Settings.isFeiBai = false;
        SceneManager.LoadScene("CreateLevel1");
    }


    private void OnValueChangedWidth(float value)
    {
        Settings.width = value;
        point.transform.localScale=Vector3.one*value*0.3f;
    }
}
