using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TargetManager : MonoBehaviour
{

    public string nextSceneName;
    
    private List<Target> targets=new List<Target>();

    private static TargetManager _instance;
    private TargetManager _targetManagerInstance;
    
    private int index = 0;
    
    public Button nextButton;
    
    private List<List<AreaBase>> targetsList = new List<List<AreaBase>>();


    private void Start()
    {

        if (!Settings.isMiaoHong)
        {
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
            return;
        }
        
        List<GameObject> roundObjects = FindAllRoundObjects();
        
        
        foreach (GameObject obj in roundObjects)
        {
            int number = GetNumberFromName(obj.name);

            foreach (var area in obj.GetComponentsInChildren<AreaBase>(true))
            {
                
                RegisterArea(number, area);
            }
        }
        
        
        
        index = 0;

        foreach (var target in targetsList[index])
        {
            target.gameObject.SetActive(true);
            if(target is Target target1)targets.Add(target1);
            //Debug.Log(index);
        }
        
    }

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


    public void RegisterArea(int i, AreaBase area)
    {
        while (i>targetsList.Count-1)
        {
            targetsList.Add(new List<AreaBase>());
        }

        targetsList[i].Add(area);
        
        area.gameObject.SetActive(false);
        
        Debug.Log(i);
    }
    
    public void RemoveTarget(Target target)
    {
        if(targets.Contains(target))targets.Remove(target);
    }


    private void ResetScore()
    {
        targets.Clear();
        targetsList[index].ForEach((target)=>
        {
            target.OnReset();
            if(target is Target t)targets.Add(t);
        });
        
    }
    
    public void ResetGame()
    {
        var ran=Random.Range(1, 3);
        AudioManager.Instance.PlayAudio($"错误{ran}");
        if (Settings.isDuanBi)
        {
            return;
        }
        if (Settings.isMiaoHong)
        {
            ResetScore();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        

    }
    
    public bool EndGame()
    {
        if (targets.Count == 0)
        {
            var ran=Random.Range(1, 3);
            AudioManager.Instance.PlayAudio($"扫{ran}");
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

        if (index == targetsList.Count - 1)
        {

            nextButton.gameObject.SetActive(true);
            nextButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
            foreach (var target in targetsList[index])
            {
                target.gameObject.SetActive(false);
            }
        }
        else
        {


            foreach (var target in targetsList[index])
            {
                target.gameObject.SetActive(false);
            }

            index++;

            if (targetsList[index].Count == 0)
            {
                NextLevel();
                return;
            }

            foreach (var target in targetsList[index])
            {
                target.gameObject.SetActive(true);
                if(target is Target target1)targets.Add(target1);
            }
        }
    }

    List<GameObject> FindAllRoundObjects()
        {
            List<GameObject> roundObjects = new List<GameObject>();
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (Regex.IsMatch(obj.name, @"^Round\d+$"))
                {
                    roundObjects.Add(obj);
                }
            }

            return roundObjects;
        }
    
    int GetNumberFromName(string name)
    {
        Match match = Regex.Match(name, @"\d+$");
        if (match.Success)
        {
            return int.Parse(match.Value);
        }
        else
        {
            return -1;
        }
    }
    
}
