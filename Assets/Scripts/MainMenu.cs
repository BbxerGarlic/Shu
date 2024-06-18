using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [FormerlySerializedAs("feiBaiButton")] public Button miaoHongButton;
    [FormerlySerializedAs("normalButton")] public Button createButton;
    [FormerlySerializedAs("guideButton")] public Button guideButton;

    private void Start()
    {
        // 设置按钮的点击事件
        miaoHongButton.onClick.AddListener(OnMiaoHongButtonClicked);
        createButton.onClick.AddListener(OnCreateButtonClicked);
        guideButton.onClick.AddListener(OnGuideButtonClicked);

    }

    public void OnMiaoHongButtonClicked()
    {
        Settings.isMiaoHong = true;
        SceneManager.LoadScene("MiaoHongLevel1");
    }

    public void OnCreateButtonClicked()
    {
        Settings.isMiaoHong = false;
        SceneManager.LoadScene("CreateMenu");
    }
    public void OnGuideButtonClicked()
    {
        Settings.isMiaoHong = false;
        SceneManager.LoadScene("GuideLevel1");
    }


    
}


public static class Settings
{
    public static bool isFeiBai=true;

    public static bool isMiaoHong;
    
    public static bool isDuanBi=true;

    public static float width=2f;
}

public class ListExpander
{
    public static void EnsureListSize<T>(List<T> list, int targetSize, T defaultValue = default(T))
    {
        if (list == null)
        {
            list = new List<T>(targetSize);
        }

        while (list.Count <= targetSize)
        {
            list.Add(defaultValue);
        }
    }
}