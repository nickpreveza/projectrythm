using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnboardingPanel : UIPanel
{
    [Header("OnBoarding Panel Settings")]
    [SerializeField] GameObject[] tutorialSlides;
    [SerializeField] int startingSlideDebug;
    [SerializeField] GameObject[] confirmButtons;
    [SerializeField] List<AnticipationPoint> q2_anticipationPointsPool = new List<AnticipationPoint>();
    [SerializeField] List<AnticipationPoint> q2_occupiedPointsPool = new List<AnticipationPoint>();

    [SerializeField] List<AnticipationPoint> q3_anticipationPointsPool = new List<AnticipationPoint>();
    [SerializeField] List<AnticipationPoint> q3_occupiedPointsPool = new List<AnticipationPoint>();

    bool quest1Active; //get 3 perfect hits
    bool quest2Active; //get 3 perfect hits from moving points
    bool quest3Active; //get a combo of 6
    bool quest4Active; //reach multiplier x2

    float perfectHits;

    [SerializeField] TextMeshProUGUI quest1text;
    [SerializeField] TextMeshProUGUI quest2text;
    [SerializeField] TextMeshProUGUI quest3text;
    [SerializeField] TextMeshProUGUI quest4text;

    public AnticipationPoint selectedPoint;
    AnticipationPoint nextPoint;

    [SerializeField] UIPoint spawnedUIPoint;

    bool spawmMeteorites;
    [SerializeField] TextMeshProUGUI fakeComboText;
    [SerializeField] Image fakeMultiplierFill;

    [SerializeField] int comboGoal = 6;
    [SerializeField] int destructionScore = 1;
    int maxGauge = 20;
    int currentGauge = 1;
    private void Start()
    {
        GameManager.OnPointHit += OnPointHit;
        GameManager.OnPointMissed += OnPointMissed;
        GameManager.OnBuildingDestroyed += OnBuildingDestroyed;
    }

    public void OnPointMissed(AnticipationPoint pointParent)
    {
        pointParent.HideAnticipationImage();
       

        if (quest2Active)
        {
            if (pointParent != null)
            {
                q2_occupiedPointsPool.Remove(pointParent);
                q2_anticipationPointsPool.Add(pointParent);

                if (selectedPoint == pointParent)
                {
                    selectedPoint = null;
                }
            }

            SpawnRandomPoint();
        }
     

        if (quest3Active)
        {
            if (pointParent != null)
            {
                q3_occupiedPointsPool.Remove(pointParent);
                q3_anticipationPointsPool.Add(pointParent);

                if (selectedPoint == pointParent)
                {
                    selectedPoint = null;
                }
            }

            perfectHits = 0;
            UpdateFakeCombo();
            SpawnRandomPoint();
        }
       
    }

    void UpdateFakeCombo()
    {
        quest3text.text = "• Reach a Combo: " + perfectHits + " / " + comboGoal + " • ";
        fakeComboText.text = "x" + perfectHits;
    }

    void UpdateFakeGauge()
    {
        quest4text.text = "• Reach Max Multiplier •";
        fakeMultiplierFill.fillAmount = (float)currentGauge / (float)maxGauge;
    }

    IEnumerator SpawnMeteorites()
    {
        while (spawmMeteorites)
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.SpawnMeteorites(true);
        }
      
    }

    public void OnBuildingDestroyed()
    {
        if (quest4Active)
        {
            currentGauge += destructionScore;

            UpdateFakeGauge();

            if (currentGauge >= maxGauge)
            {
                currentGauge = 1;
                perfectHits = 0;
                Next(8);
            }
        }
    }

    public void OnPointHit(TargetState pointState, AnticipationPoint pointParent)
    {
       
        if (isActive)
        {
            if (quest1Active)
            {
                if (pointState == TargetState.PERFECT)
                {
                    perfectHits++;
                    quest1text.text = "• Get 3 Perfect Hits: " + perfectHits + " / 3 • ";

                    if (perfectHits == 3)
                    {
                        perfectHits = 0;
                        Next(2);
                        return;
                    }
                }
               
            }

            if (quest2Active)
            {
                pointParent.HideAnticipationImage();
                if (pointState == TargetState.PERFECT)
                {
                    perfectHits++;
                    quest2text.text = "• Get 3 Perfect Hits: " + perfectHits + " / 3 • ";

                    if (perfectHits == 3)
                    {
                        Next(4);
                        return;
                    }
                    
                }

                q2_occupiedPointsPool.Remove(pointParent);
                q2_anticipationPointsPool.Add(pointParent);

                selectedPoint = null;

                SpawnRandomPoint();
            }

            if (quest3Active)
            {
                pointParent.HideAnticipationImage();
                perfectHits++;
                UpdateFakeCombo();
                q3_occupiedPointsPool.Remove(pointParent);
                q3_anticipationPointsPool.Add(pointParent);

                selectedPoint = null;
                if (perfectHits >= comboGoal)
                {
                    Next(6);
                    return;
                }

                SpawnRandomPoint();
            }
        }
    }
    void CloseAllPanels()
    {
        foreach(AnticipationPoint point in q2_anticipationPointsPool)
        {
            point.HideAnticipationImage();
        }

        foreach (AnticipationPoint point in q3_anticipationPointsPool)
        {
            point.HideAnticipationImage();
        }
        foreach (GameObject obj in tutorialSlides)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in confirmButtons)
        {
            obj.SetActive(false);
        }
    }

    public override void Setup()
    {
        perfectHits = 0;
        base.Setup();
    }

    public override void Activate()
    {
        CloseAllPanels();
        perfectHits = 0;
        quest1Active = false;
        quest2Active = false;
        quest3Active = false;
        quest4Active = false;
        tutorialSlides[0].SetActive(true);
        GameManager.Instance.godHandler?.DisableGod();
       
        if (GameManager.Instance.isDebug)
        {
            Next(startingSlideDebug);
        }
        base.Activate();

    }

    public override void Disable()
    {
        quest1Active = false;
        quest2Active = false;
        quest3Active = false;
        quest4Active = false;
        perfectHits = 0;
        base.Disable();
    }

    public void Restart()
    {
        //no restart yet
    }

    public void GoToMenu()
    {
        quest1Active = false;
        quest2Active = false;
        quest3Active = false;
        quest4Active = false;
        perfectHits = 0;
        CloseAllPanels();
        GameManager.Instance.planetHandler.OnboardingExit();
        GameManager.Instance.DestroyAllMeteors();
        UIManager.Instance.OpenMainMenu();
    }

    public void Next(int index)
    {
        if (index >= tutorialSlides.Length)
        {
            UIManager.Instance.OpenMainMenu();
            return;
        }

        perfectHits = 0;
        CloseAllPanels();
        tutorialSlides[index].SetActive(true);
        confirmButtons[0].SetActive(true);
        confirmButtons[1].SetActive(true);

        //0 quest intro
        //1 hit points 3 times
        //2 quest 2 intro
        //3 hit random points
        //4 quest 3 intro
        //5 destroy buildings with meteorites
        //6 end

        switch (index)
        {
            case 1: 
                quest1Active = true; //hit same point 
                break;
            case 2:
                quest1Active = false;
                break;
            case 3: 
                quest2Active = true;  //hit random points 
                foreach(AnticipationPoint point in q2_anticipationPointsPool)
                {
                    point.HideAnticipationImage();
                }
                selectedPoint = null;
                nextPoint = null;
                SpawnRandomPoint();
                break;
            case 4:
                quest2Active = false;
                quest3Active = false;
                selectedPoint = null;
                nextPoint = null;
                foreach(AnticipationPoint point in q2_occupiedPointsPool)
                {
                    if (!q2_anticipationPointsPool.Contains(point))
                    {
                        q2_anticipationPointsPool.Add(point);
                    }
                }
                perfectHits = 0;
                comboGoal = 6;
                break;
            case 5: //get a combo of 6 or something
                quest3Active = true;
                foreach (AnticipationPoint point in q3_anticipationPointsPool)
                {
                    point.HideAnticipationImage();
                }
                selectedPoint = null;
                nextPoint = null;

                SpawnRandomPoint();
                UpdateFakeCombo();
                break;
            case 6:
                quest3Active = false;
                selectedPoint = null;
                nextPoint = null;
                foreach (AnticipationPoint point in q3_occupiedPointsPool)
                {
                    if (!q3_anticipationPointsPool.Contains(point))
                    {
                        q3_anticipationPointsPool.Add(point);
                    }
                }
                break;
            case 7: //destory buildings with meteorites
                quest4Active = true;
                UpdateFakeGauge();
                GameManager.Instance.planetHandler.OnboardingStart();

                spawmMeteorites = true;
                GameManager.Instance.SpawnMeteorites(true);
                StartCoroutine(SpawnMeteorites());
                break;
            case 8:
                quest4Active = false;
                spawmMeteorites = false;
                StopAllCoroutines();
                GameManager.Instance.planetHandler.OnboardingExit();
                GameManager.Instance.DestroyAllMeteors();
                break;
        }
       
    }


    public void SpawnRandomPoint()
    {
        List<AnticipationPoint> selectedPool;
        List<AnticipationPoint> occupiedPool;
        if (quest2Active)
        {
            selectedPool = q2_anticipationPointsPool;
            occupiedPool = q2_occupiedPointsPool;
        }
        else
        {
            selectedPool = q3_anticipationPointsPool;
            occupiedPool = q3_occupiedPointsPool;
        }

        if (selectedPoint == null && nextPoint == null)
        {
            int randomPoint = Random.Range(0, selectedPool.Count);
            selectedPoint = selectedPool[randomPoint];
            occupiedPool.Add(selectedPoint);
            selectedPool.Remove(selectedPoint);

            selectedPoint.ShowAnticipationImage();
            selectedPoint.SpawnPoint(1.6f);

            int randomPointNext = Random.Range(0, selectedPool.Count);
            nextPoint = selectedPool[randomPointNext];
            occupiedPool.Add(nextPoint);
            selectedPool.Remove(nextPoint);

            nextPoint.ShowAnticipationImage();
        }
        else if (selectedPoint == null && nextPoint != null)
        {
            selectedPoint = nextPoint;
            selectedPoint.ShowAnticipationImage();
            selectedPoint.SpawnPoint(1.6f);

            int randomPoint = Random.Range(0, selectedPool.Count);
            nextPoint = selectedPool[randomPoint];
            occupiedPool.Add(nextPoint);
            selectedPool.Remove(nextPoint);

            nextPoint.ShowAnticipationImage();
        }

        if (quest2Active)
        {
            q2_anticipationPointsPool = selectedPool;
            q2_occupiedPointsPool = occupiedPool;
        }
        else
        {
             q3_anticipationPointsPool = selectedPool;
             q3_occupiedPointsPool = occupiedPool;
        }

    }

    public void ShowConfirmButton(int index)
    {
        confirmButtons[index].SetActive(true);
    }

    public void ConfirmDestruction()
    {
       
    }

}
