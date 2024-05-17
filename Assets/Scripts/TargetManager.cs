using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetManager : MonoBehaviour
{

    public string nextSceneName;
    
    private List<Target> targets=new List<Target>();

    private static TargetManager _instance;
    private TargetManager _targetManagerInstance;

    // 获取 TargetManagerSingleton 的单例
    public static TargetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TargetManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("TargetManager");
                    _instance = go.AddComponent<TargetManager>();
                }
            }
            return _instance;
        }
    }

    public void AddTarget(Target target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(Target target)
    {
        if(targets.Contains(target))targets.Remove(target);
    }

    public bool EndGame()
    {
        if (targets.Count == 0)
        {
            Invoke(nameof(NextLevel), 1f);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }
    
    
}
