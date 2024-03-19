using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScorePanel : UIPanel
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI underHeader;
    [SerializeField] TextMeshProUGUI perfectHits;
    [SerializeField] TextMeshProUGUI misses;
    [SerializeField] TextMeshProUGUI maxCombo;
    [SerializeField] TextMeshProUGUI buildingsDestoryed;
    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI finalGrade;
    [SerializeField] TextMeshProUGUI nextRankGrade;
    public override void Setup()
    {
        perfectHits.text = "PERFECT HITS: " + ScoreManager.Instance.totalSessionPerfectHits;
        misses.text = "MISSED HITS: " + ScoreManager.Instance.totalSessionMisses;
        maxCombo.text = "MAX COMBO: " + ScoreManager.Instance.maxSessionCombo;
        buildingsDestoryed.text = "DESTRUCTION: " + ScoreManager.Instance.totalSessionBuildingsDestroyed;
        finalScore.text = ScoreManager.Instance.currentScore.ToString();
        ScoreManager.Instance.CalculateGrade();
        finalGrade.text = ScoreManager.Instance.rank;
        if (ScoreManager.Instance.nextRankThreshold != "S")
        {
            nextRankGrade.text = "Next Rank At " + ScoreManager.Instance.nextRankThreshold;
        }
        else
        {
            nextRankGrade.text = "Max Rank Achieved";
        }
     
        base.Setup();
    }

    public void UpdateScoreElements(bool isWin)
    {
        if (isWin)
        {
            title.text = "FINAL SCORE";
            underHeader.text = "COMPLETED!";
        }
        else
        {
            title.text = "GAME OVER";
            nextRankGrade.text = "You missed too many!";
            underHeader.text = "TRY AGAIN!";
            finalGrade.text = "F";
        }
    }

    public override void Activate()
    {
        base.Activate();

    }

    public override void Disable()
    {
        base.Disable();
    }

    public void Restart()
    {
        //no restart yet
    }

    public void MainMenu()
    {
        SaveManager.Instance.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
