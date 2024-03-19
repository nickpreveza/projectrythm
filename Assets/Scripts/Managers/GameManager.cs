using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class GameManager : MonoBehaviour
{
    float TimeLeft = 5;
    bool TimerOn = false;
    bool hasPlayed = false;
    bool fix;
    public static string resultThemeString;

    public static GameManager Instance;
   
    [HideInInspector]
    public VisualsManager visuals;
    [Header("Game Data")]
    [SerializeField] GameData data;
    [Header("Game Settings")]
    public bool isDebug;
    public int rotationSpeedMultiplier; //planet Rotation Speed
    [Range(0,1)]
    public float meteorSpawnChance;
    public bool is_paused;
    public bool gameHasStarted;
    public GameState State;

    [Header("Assign These")]
    public GameObject pointCanvas; //the canvas that target points are spawned
    public PointHandler pointHandler; //the handler that generaetes points
    public GameObject kinect; //the kinect gameObject
    //[SerializeField] GaugeInterraction gaugeItem;
    public GameObject pointPrefabUI; //new targetPoint
    public Volume globalVolume;
    public PlanetHandler planetHandler;
    GameState tempState;
    List<Meteor_Spawning> meteorSpawners = new List<Meteor_Spawning>();

    //Events
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<TargetState, AnticipationPoint> OnPointHit;
    public static event Action<AnticipationPoint> OnPointMissed;
    public static event Action OnBuildingDestroyed;

    [SerializeField] GameObject hardLevel;
    [SerializeField] GameObject easyLevel;
    public GameObject destructionParticleEffect;
    public int currentLevel = 0;
    public GodHandler godHandler;
    private void Awake()
    {
        Instance = this;
        visuals = GetComponent<VisualsManager>();
       
    }

    private void Start()
    {
        UpdateGameState(GameState.START);
        DepthOfField dof;
        if (globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            dof.active = false;
        }


        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON, so start at index 1.
        // Check if additional displays are available and activate each.

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        float debugCombo = 0;
        float totalScore = 0;
        bool mistake1 = false;
        for(int i = 1; i < 188; i++)
        {
            debugCombo++;
            if (!mistake1)
            {
                if (debugCombo == 50)
                {
                    debugCombo = 1;
                    mistake1 = true;
                }
                
            }
           
            if (debugCombo == 101)
            {
                debugCombo = 1;
            }
            float debugScore = 20 + debugCombo;
            totalScore += debugScore * 2;
        }

        Debug.Log("Perfect Score is " + totalScore);
    }

    public void PointHit(TargetState pointState, AnticipationPoint pointParent)
    {
        switch (pointState)
        {
            case TargetState.VERYEARLY:
                Debug.LogError("This shouldn't be possible");
                break;
            case TargetState.EARLY:
                CameraShake.instance.Shake(0.1f, 1,0.75f);
                //Apply early points
                break;
            case TargetState.PERFECT:
                CameraShake.instance.Shake(0.1f, 1,1f);
                //Apply perfect points
                break;
            case TargetState.LATE:
                CameraShake.instance.Shake(0.1f, 1,0.75f);
                //Apply late points
                break;
        }

       

        OnPointHit?.Invoke(pointState, pointParent);
    }

    public void BuildingDestroyed()
    {
        AudioManager.Instance.PlayRandomExplosionAudioSource(AudioManager.RandomPitch(0.8f, 1.2f), AudioManager.explosionSoundVolume);
        //AudioManager.Instance.Play("explosion", AudioManager.RandomPitch(0.8f, 1.2f), AudioManager.explosionSoundVolume);
        //AudioManager.Instance.Play("grabBuilding", AudioManager.RandomPitch(0.8f, 1.2f));

        OnBuildingDestroyed?.Invoke();
    }
    public void TryRegisterMiss(AnticipationPoint pointParent)
    {
        OnPointMissed?.Invoke(pointParent);
        Debug.Log("Point Missed");
    }
    public bool SetPause
    {
        get
        {
            return is_paused;
        }
        set
        {
            is_paused = value;
            if (is_paused)
            {

                Time.timeScale = 0.02f;
                //AudioManager.Instance.PauseLayers();
                
                AudioManager.Instance.PauseLayers();

                List<Joycon> joycons = JoyconManager.Instance.j;
                if (joycons != null && joycons.Count > 0)
                {
                    joycons[0]?.SetRumble(0, 0, 0, 0);
                }
               
}
            else
            {

                Time.timeScale = 1;
                AudioManager.Instance.UnpauseLayers();
            }
            UIManager.Instance.PauseChanged();
        }
    }
    private void Update()
    {   
        if (TimerOn)
        {
            TimeLeft = TimeLeft - Time.fixedDeltaTime;
            if (TimeLeft <= 0.0f)
            {
                TimeLeft = 0;
                if (!hasPlayed)
                    AudioManager.Instance.Play(resultThemeString);
                hasPlayed = true;
            }
        }

        if (State == GameState.GAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (is_paused)
                {
                    SetPause = false;
                }
                else
                {
                    SetPause = true;
                }
            }

        }


        if (isDebug && gameHasStarted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                EndRound();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                GameOver();
            }
        }
       
    }

    public void EndRound()
    {
        UIManager.Instance.leaderboardHandler.ShowHighScorePopup();
        UpdateGameState(GameState.END);
        TimerOn = true;
        AudioManager.Instance.Play("resultFeedback");
    }

    public void GameOver()
    {
        UIManager.Instance.leaderboardHandler.ShowHighScorePopup();
        UpdateGameState(GameState.GAMEOVER);
        TimerOn = true;
        TimeLeft = 2.05f;
        AudioManager.Instance.Play("resultFeedback");
    }


    public void AddMeteorSpawner(Meteor_Spawning spawner)
    {
        meteorSpawners.Add(spawner);
    }

    public void DestroyAllMeteors()
    {
        /* //game breaking
        foreach(GameObject obj in activeMeteors)
        {
            if (obj != null)
            {
                obj.GetComponent<MeteorManager>().DestroyWithCoolness();
            }

        }*/
    }
    public void SpawnMeteorites(bool skipChances)
    {
        if (!skipChances)
        {
            if (UnityEngine.Random.Range(0f, 1f) < meteorSpawnChance)
            {
                int randomSpawner = UnityEngine.Random.Range(0, meteorSpawners.Count);
                meteorSpawners[randomSpawner].SpawnMeteor();
            }
        }
        else
        {
            int randomSpawner = UnityEngine.Random.Range(0, meteorSpawners.Count);
            meteorSpawners[randomSpawner].SpawnMeteor();
        }
      
      
    }

    public void SetGaugeState(bool active)
    {
        //gaugeItem.gameObject.SetActive(active);
    }

    public void StartGame(int level)
    {
        
        switch (level)
        {
            case 1:
                hardLevel.SetActive(false);
                easyLevel.SetActive(true);
                break;
            case 2:
                hardLevel.SetActive(true);
                easyLevel.SetActive(false);
                break;

        }

        currentLevel = level;
        UpdateGameState(GameState.GAME);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.DEFAULT:
                UIManager.Instance.OpenMainMenu();
                break;
            case GameState.START:
                pointCanvas.SetActive(false);
                UIManager.Instance.OpenMainMenu();
                break;
            case GameState.GAME:
                pointCanvas.SetActive(true);
                //gaugeItem.gameObject.SetActive(true);
                UIManager.Instance.OpenInGamePanel();
                if (!gameHasStarted)
                {
                    Time.timeScale = 1f;
                   

                    AudioManager.Instance.StartGame();
                    //Level_Designer.Instance.StartGame();
                    gameHasStarted = true;
                }
                break;
            case GameState.END: //song ended
                AudioManager.Instance.StopLayers();
                AudioManager.levelMusicSource.Stop();
                gameHasStarted = false;
                pointCanvas.GetComponent<CanvasGroup>().alpha = 0;
                planetHandler.GameFinale(true);
                UIManager.Instance.OpenEndGamePanel(true);
                break;
            case GameState.GAMEOVER: //player lost
                AudioManager.Instance.StopLayers();
                AudioManager.levelMusicSource.Stop();
                gameHasStarted = false;
                pointCanvas.GetComponent<CanvasGroup>().alpha = 0;
                UIManager.Instance.OpenEndGamePanel(false);
                break;

        }

        OnGameStateChanged?.Invoke(newState);
        
    }

 
    public void DataLoaded()
    {
        UIManager.Instance.DataLoaded();
    }

}

public enum GameState
{
    DEFAULT,
    START, //menu
    GAME, //ingame
    END, //endofSong
    GAMEOVER //failOfSong
}



public enum ActiveScene
{
    MENU,
    GAME
}
