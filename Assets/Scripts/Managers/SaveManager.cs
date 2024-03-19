using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] GameData data;
    public bool canLoad;
    bool shouldLoad = false;
    bool setUpInProgress;
    [SerializeField] ActiveScene currentScene;
    public GameData publicData
    {
        get
        {
            return data;
        }
        private set
        {
            Debug.Log("Can't direclty set game Data, call related funciton");
        }

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        SetUpPersistentData();

        Time.timeScale = 1;
    }

    void SetUpPersistentData()
    {


        Load();

    }

    public void SaveNewHighscore()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ForceCleanSlate();
            }
        }
    }

    public void ForceCleanSlate()
    {
        CreatePrefs();
        Save();
        Load();
    }
    void CreatePrefs()
    {
        data.leaderboardNames = new List<string>();
        data.leaderboardScores = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            data.leaderboardNames.Add("UNCLAIMED");
            data.leaderboardScores.Add(0);
        }
        Save();
    }

    public void Save()
    {
        string saveJson = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Save", saveJson);
        PlayerPrefs.Save();
    }

    public void Load()
    {

        if (PlayerPrefs.HasKey("Save"))
        {

            string saveJson = PlayerPrefs.GetString("Save");
            data = JsonUtility.FromJson<GameData>(saveJson);

        }
        else
        {
            CreatePrefs();
        }

        if (data.leaderboardScores.Count < 10)
        {
            CreatePrefs();
        }
       

        GameManager.Instance.DataLoaded();
    }
}

