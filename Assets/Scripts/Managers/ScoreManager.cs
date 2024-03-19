using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Note Points")]
    [SerializeField] int earlyPointsReward;
    [SerializeField] int perfectPointsReward;
    [SerializeField] int latePointsReward; //unused

    [SerializeField] float destructionPointsReward; //given from destructions - ups the gauge
    [SerializeField] int pointsFromDestruciton; //this should be 0 but whatever;
    [Header("Level 1: Grading Thresholds")]
    [SerializeField] int gradeS;
    [SerializeField] int gradeA;
    [SerializeField] int gradeB;
    [SerializeField] int gradeC;
    [SerializeField] int gradeD;
    [SerializeField] int gradeE;

    [Header("Level 2: Grading Thresholds")] //h stands for Hard Level
    [SerializeField] int hGradeS;
    [SerializeField] int hGradeA;
    [SerializeField] int hGradeB;
    [SerializeField] int hGradeC;
    [SerializeField] int hGradeD;
    [SerializeField] int hGradeE;

    [Header("Settings & Requirments")]
    public float gaugeMaxDestructionPoints; //total in the bar
    [SerializeField] int missesForGameOver = 5; //consecutive
    [SerializeField] int allowed_misses_per_layer = 2;
    [SerializeField] int required_hits_to_advance_music = 5;
    [SerializeField] int hitsForMeteor;

    [SerializeField] float timeToWaitBeforeFall;
    float internalTimeToWait;
    [SerializeField] float destructionFallingSpeed;

    [Range(0, 1)]
    [SerializeField] float multiplier1;
    [Range(0, 1)]
    [SerializeField] float multiplier2;
    [Range(0, 1)]
    [SerializeField] float multiplier3;

    [Header("Session Values")]
    public float currentMultiplier = 1;
    public int currentCombo = 1;
    public int currentScore = 0;
    public float currentDestructionPoints;
    public int currentLayer = 1;
    public int currentMisses = 0;
    public int currentLayerMisses = 0;
    public int currentLayerPerfectHits = 0;
    public int totalSessionPerfectHits;
    public int totalSessionMisses;
    public int totalSessionLateHits;
    public int totalSessionEarlyHits;
    public int maxSessionCombo;
    public int totalSessionBuildingsDestroyed;

    public bool destroyBuildingsAbility = false;
    [SerializeField] bool grabMeteorsAbility = true;
    [SerializeField] bool grabGaugeAbility = false;

    public float gaugePercentage;

    bool gaugeCooldown;
    bool gaugeFalling;

    public string rank;
    public string nextRankThreshold;

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
    }

    void Start()
    {
        currentCombo = 1;
        currentScore = 0;
        maxSessionCombo = 0;
        GameManager.OnPointHit += OnPointHit;
        GameManager.OnPointMissed += OnPointMissed;
        GameManager.OnBuildingDestroyed += OnBuildingDestroyed;
       // currentDestructionPoints = gaugeMaxDestructionPoints;
        gaugeFalling = true;
    }

    private void OnDestroy()
    {
        GameManager.OnPointHit -= OnPointHit;
        GameManager.OnPointMissed -= OnPointMissed;
        GameManager.OnBuildingDestroyed -= OnBuildingDestroyed;
    }

    void OnBuildingDestroyed()
    {
        if (GameManager.Instance.gameHasStarted)
        {
            totalSessionBuildingsDestroyed++;
            AddDestructionPoints();
            AddPointsFromDestruction();
        }
    }

    void OnPointHit(TargetState pointState, AnticipationPoint anticipationpoint)
    {
        if (GameManager.Instance.gameHasStarted)
        AddPoints(pointState);
        currentMisses = 0;
    }

    void OnPointMissed(AnticipationPoint anticipationpoint)
    {
        if (GameManager.Instance.gameHasStarted)
        Miss();
            
    }

    void AddPointsFromDestruction()
    {
        currentScore += pointsFromDestruciton;
        UIManager.Instance.UpdateScore();
    }

    void AddPoints(TargetState pointState)
    {
        int amountToAdd = 0;
        switch (pointState)
        {
            case TargetState.EARLY:
                totalSessionEarlyHits++;
                amountToAdd = earlyPointsReward;
                break;
            case TargetState.PERFECT:
                totalSessionPerfectHits++;
                currentLayerPerfectHits++;
               // AudioManager.Instance.Play("perfectHit", AudioManager.RandomPitch(0.95f, 1.05f));
                GameManager.Instance.SpawnMeteorites(false);
                amountToAdd = perfectPointsReward;
                break;
            case TargetState.LATE:
                totalSessionLateHits++;
                amountToAdd = latePointsReward;
                break;
        }

        currentCombo++;
        currentScore += (int)(currentMultiplier * (amountToAdd + currentCombo));
   
        if (currentCombo > maxSessionCombo)
        {
            maxSessionCombo = currentCombo;
        }

        CheckLayer();
        UIManager.Instance.UpdateScore();
    }

    public void AddDestructionPoints()
    {
        currentDestructionPoints = Mathf.Clamp(currentDestructionPoints + destructionPointsReward, 0, gaugeMaxDestructionPoints);
        internalTimeToWait = timeToWaitBeforeFall;
        gaugeCooldown = true;
        gaugeFalling = false;
        CheckDestructionsPoints();
    }

    public int FindPlacement()
    {

        for (int i = 0; i < SaveManager.Instance.publicData.leaderboardScores.Count; i++)
        {
            if (currentScore > SaveManager.Instance.publicData.leaderboardScores[i])
            {
                return i;
            }
        }

        return -1;
    }

    public void UpdateLeaderboardData()
    {
        int indexToReplace = -1;

       // List<string> tempNames = new List<string>(SaveManager.Instance.publicData.leaderboardNames);
        //List<int> tempScores = new List<int>(SaveManager.Instance.publicData.leaderboardScores);

        for(int i = 0; i < SaveManager.Instance.publicData.leaderboardScores.Count; i++)
        {
            if (currentScore > SaveManager.Instance.publicData.leaderboardScores[i])
            {
                indexToReplace = i;
                break;
            }
        }

        if (indexToReplace < 0)
        {
            return;
        }

        SaveManager.Instance.publicData.leaderboardNames.Insert(indexToReplace, SaveManager.Instance.publicData.playerName);
        SaveManager.Instance.publicData.leaderboardScores.Insert(indexToReplace, currentScore);
        UIManager.Instance.leaderboardHandler.UpdateScores();
        SaveManager.Instance.Save();
    }

    private void Update()
    {
        if (GameManager.Instance.gameHasStarted)
        {
            gaugePercentage = currentDestructionPoints / gaugeMaxDestructionPoints;

            if (gaugeCooldown)
            {
                internalTimeToWait -= Time.deltaTime;
                if (internalTimeToWait <= 0)
                {
                    gaugeCooldown = false;
                    gaugeFalling = true;
                }
            }
            if (gaugeFalling)
            {
                currentDestructionPoints -= Mathf.Clamp(destructionFallingSpeed * Time.deltaTime, 0f, gaugeMaxDestructionPoints);
                CheckDestructionsPoints();
            }
        }
      
    }

    public void CheckDestructionsPoints()
    {
        //check the percentage of the destruction points and check thresholds
        float percentage = currentDestructionPoints / gaugeMaxDestructionPoints;

        if (percentage < multiplier1)
        {
            currentMultiplier = 0.5f;
        }
        else if (percentage >= multiplier1 && percentage < multiplier2)
        {
            currentMultiplier = 1;
        }
        else if (percentage >= multiplier2 && percentage < multiplier3)
        {
            currentMultiplier = 1.5f;
        }
        else if (percentage >= multiplier3)
        {
            currentMultiplier = 2;
        }
    }

    void CheckLayer()
    {
        if (currentLayer < 5 && currentLayerPerfectHits > required_hits_to_advance_music)
        {
            currentLayerPerfectHits = 0;
            currentLayer++;
        }

        if (currentLayer > 1 && currentLayerMisses > allowed_misses_per_layer)
        {
            currentLayerMisses = 0;
            currentLayer--;
        }
    }

    public void Miss()
    {
        totalSessionMisses++;
        currentMisses++;
        currentCombo = 1;
        currentLayerMisses++;
        currentLayerPerfectHits = 0;
        AudioManager.Instance.Play("targetMiss", AudioManager.RandomPitch(0.7f, 1.3f), AudioManager.targetMissSoundVolume);
        CheckLayer();
        UIManager.Instance.UpdateScore();

        if (currentMisses >= missesForGameOver)
        {
            GameManager.Instance.GameOver();
        }
    }

    public string CalculateGrade()
    {
        string tempRank;
        string tempNext = "";

        if (GameManager.Instance.currentLevel == 1)
        {
            if (currentScore < gradeE)
            {
                tempRank = "F";
                tempNext = gradeE.ToString();
            }
            else if (currentScore >= gradeE && currentScore < gradeD)
            {
                tempRank = "E";
                tempNext = gradeD.ToString();
            }
            else if (currentScore >= gradeD && currentScore < gradeC)
            {
                tempRank = "D";
                tempNext = gradeC.ToString();
            }
            else if (currentScore >= gradeC && currentScore < gradeB)
            {
                tempRank = "C";
                tempNext = gradeB.ToString();
            }
            else if (currentScore >= gradeB && currentScore < gradeA)
            {
                tempRank = "B";
                tempNext = gradeA.ToString();
            }
            else if (currentScore >= gradeA && currentScore < gradeS)
            {
                tempRank = "A";
                tempNext = gradeS.ToString();
            }
            else if (currentScore >= gradeS)
            {
                tempRank = "S";
                tempNext = "S";
            }
            else
            {
                Debug.LogWarning("Score does not meet any threshold");
                tempRank = "?";
            }
        }
        else 
        {
            if (currentScore < hGradeE)
            {
                tempRank = "F";
                tempNext = hGradeE.ToString();
            }
            else if (currentScore >= hGradeE && currentScore < hGradeD)
            {
                tempRank = "E";
                tempNext = hGradeD.ToString();
            }
            else if (currentScore >= hGradeD && currentScore < hGradeC)
            {
                tempRank = "D";
                tempNext = hGradeC.ToString();
            }
            else if (currentScore >= hGradeC && currentScore < hGradeB)
            {
                tempRank = "C";
                tempNext = hGradeB.ToString();
            }
            else if (currentScore >= hGradeB && currentScore < hGradeA)
            {
                tempRank = "B";
                tempNext = hGradeA.ToString();
            }
            else if (currentScore >= hGradeA && currentScore < hGradeS)
            {
                tempRank = "A";
                tempNext = hGradeS.ToString();
            }
            else if (currentScore >= hGradeS)
            {
                tempRank = "S";
                tempNext = "S";
            }
            else
            {
                Debug.LogWarning("Score does not meet any threshold");
                tempRank = "?";
            }
        }
       

        rank = tempRank;
        nextRankThreshold = tempNext;
        return rank;
    }

}
