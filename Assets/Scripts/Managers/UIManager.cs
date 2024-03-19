using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] Canvas canvas;
    public UIPanel pausePanel;
    public UIPanel ingamePanel;
    public UIPanel scorePanel;
    public UIPanel mainMenuPanel;
    public UIPanel creditsPanel;
    public UIPanel onboardingPanel;

    public bool creditsEnabled;
    public bool onboardingEnabled;
    public LeaderboardHandler leaderboardHandler;

    public GameObject videoPlayer;
    public GameObject videoPanel;
    void Awake()
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

    public void DataLoaded()
    {
        leaderboardHandler.UpdateScores();
    }

    public void RollCredits()
    {
        creditsPanel.Activate();
        creditsEnabled = true;
    }

    public void UpdateScore()
    {
        ingamePanel.GetComponent<GamePanel>().UpdateScoreCombo(ScoreManager.Instance.currentScore, ScoreManager.Instance.currentCombo);
    }


    public void OpenInGamePanel()
    {
        GameManager.Instance.SetGaugeState(true);
        mainMenuPanel.Disable();
        pausePanel.Disable();
        creditsPanel.Disable();
        onboardingPanel.Disable();

        ingamePanel.GetComponent<GamePanel>().Setup();
        ingamePanel.Activate();
    }

    public void OpenPausePanel()
    {
        pausePanel.GetComponent<PausePanel>().Setup();
        pausePanel.Activate();
    }

    public void OpenMainMenu()
    {
        GameManager.Instance.SetGaugeState(false);
        GameManager.Instance.godHandler.EnableGod();
        onboardingPanel.Disable();
        ingamePanel.Disable();
        pausePanel.Disable();
        creditsPanel.Disable();

        mainMenuPanel.Setup();
        mainMenuPanel.Activate();
    }

    public void OpenOnboarding()
    {
        ingamePanel.Disable();
        pausePanel.Disable();
        creditsPanel.Disable();
        mainMenuPanel.Disable();

        onboardingPanel.Setup();
        onboardingPanel.Activate();
    }

    public void OpenEndGamePanel(bool isWin)
    {
        pausePanel.Disable();
        ingamePanel.Disable();
        mainMenuPanel.Disable();
        creditsPanel.Disable();

        scorePanel.GetComponent<ScorePanel>().Setup();

        scorePanel.GetComponent<ScorePanel>().UpdateScoreElements(isWin);
        scorePanel.Activate();
        leaderboardHandler.UpdateScores();
    }

    public void OpenCreditsPanel()
    {
        pausePanel.Disable();
        ingamePanel.Disable();
        mainMenuPanel.Disable();
        scorePanel.Disable();

        creditsPanel.GetComponent<CreditsPanel>().Setup();
        creditsPanel.Activate();
    }

    public void PauseChanged()
    {
        if (!GameManager.Instance.gameHasStarted)
        {
            return;
        }
        if (GameManager.Instance.is_paused)
        {
            ingamePanel.canvasGroup.alpha = 0;
            pausePanel.Activate();
        }
        else
        {
            ingamePanel.canvasGroup.alpha = 1;
            pausePanel.Disable();
        }
    }
}
