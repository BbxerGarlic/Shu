using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button feiBaiButton;
    public Button normalButton;

    private void Start()
    {
        // 设置按钮的点击事件
        feiBaiButton.onClick.AddListener(FeiBaiButtonClicked);
        normalButton.onClick.AddListener(NormalButtonClicked);
    }

    private void FeiBaiButtonClicked()
    {
        StaticData.isFeiBai = true;
        SceneManager.LoadScene("Level1");
    }

    private void NormalButtonClicked()
    {
        StaticData.isFeiBai = false;
        SceneManager.LoadScene("Level1");
    }
}


public static class StaticData
{
    public static bool isFeiBai;
}
