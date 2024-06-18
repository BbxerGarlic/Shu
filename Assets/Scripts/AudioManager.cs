using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                _instance = obj.AddComponent<AudioManager>();
            }
            return _instance;
        }
    }

    public List<AudioClip> initialAudioClips; // 公共的AudioClip列表，通过Inspector赋值

    private Dictionary<string, AudioClip> audioClips;
    private List<AudioSource> audioSourcePool;
    private int poolSize = 10;

    void Awake()
    {
        // 保持单例模式
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioManager()
    {
        // 初始化音频剪辑字典
        audioClips = new Dictionary<string, AudioClip>();
        
        // 初始化音源池
        audioSourcePool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject audioObj = new GameObject("AudioSource_" + i);
            audioObj.transform.SetParent(transform);
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSourcePool.Add(audioSource);
        }

        // 加载初始音频剪辑列表中的所有音频资源
        LoadAllAudioClips();
    }

    private void LoadAllAudioClips()
    {
        foreach (AudioClip clip in initialAudioClips)
        {
            if (!audioClips.ContainsKey(clip.name))
            {
                audioClips.Add(clip.name, clip);
            }
        }
    }

    public void PlayAudio(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            AudioSource audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                audioSource.clip = audioClips[clipName];
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogError("AudioClip not loaded: " + clipName);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var audioSource in audioSourcePool)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return null;
    }
    
    // 用来记录单击的次数
    private int clickCount = 0;
    // 用来记录上次单击的时间
    private float lastClickTime = 0f;
    // 两次单击之间的最大允许时间间隔
    private const float clickInterval = 1f;
    // 所需的单击次数
    private const int requiredClicks = 5;

    void Update()
    {
        // 检测鼠标左键的单击
        if (Input.GetMouseButtonDown(0))
        {
            
            
            // 获取当前时间
            float currentTime = Time.time;
            
            // 更新上次单击的时间
            if(clickCount==0)lastClickTime = currentTime;

            // 判断当前单击是否在允许的时间间隔内
            if (currentTime - lastClickTime < clickInterval)
            {
                clickCount++;
            }
            else
            {
                // 超过时间间隔，重置单击次数
                clickCount = 0;
            }
            
            // 检查是否达到了所需的单击次数
            if (clickCount >= requiredClicks)
            {
                ExitGame();
                clickCount = 0;
            }
        }
    }

    // 退出游戏的函数
    void ExitGame()
    {
//         // 如果在编辑器中运行，停止播放模式
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #else
//         // 在构建版本中退出应用程序
//         Application.Quit();
// #endif
        SceneManager.LoadScene("MainMenu");
    }
}
