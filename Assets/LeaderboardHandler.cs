using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] leaderboardPositions;
    List<string> leaderboardNames = new List<string>();
    List<int> leaderboardScores = new List<int>();

    [SerializeField] GameObject highscorePopup;
    [SerializeField] TextMeshProUGUI placementText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button sumbitButton;
    bool inputFieldComplete;
    private void Start()
    {
        highscorePopup.SetActive(false);
    }

    public void ShowHighScorePopup()
    {
        int placement = ScoreManager.Instance.FindPlacement();

        if (placement < 0)
        {
            return;
        }

        placement++; //correcting index

        if (placement == 1)
        {
            placementText.text = "You got " + placement + "st place!";
        }
        else if (placement == 2)
        {
            placementText.text = "You got " + placement + "nd place!";
        }
        else if (placement == 3)
        {
            placementText.text = "You got " + placement + "rd place!";
        }
        else
        {
            placementText.text = "You got " + placement + "th place!";
        }

        UpdateButton();
        highscorePopup.SetActive(true);

    }

    public void ValidateInputField()
    {
        if (inputField.text.Length > 0)
        {
            inputFieldComplete = true;
            UpdateButton();
        }
    }

    void UpdateButton()
    {
        sumbitButton.interactable = inputFieldComplete;
    }

    public void Sumbit()
    {
        SaveManager.Instance.publicData.playerName = inputField.text;
        highscorePopup.gameObject.SetActive(false);
        ScoreManager.Instance.UpdateLeaderboardData();
        
    }

    public void UpdateScores()
    {
        leaderboardNames = SaveManager.Instance.publicData.leaderboardNames;
        leaderboardScores = SaveManager.Instance.publicData.leaderboardScores;

        for(int i = 0; i < leaderboardPositions.Length; i++)
        {
            if (leaderboardScores[i] == 0)
            {
                leaderboardPositions[i].text =  leaderboardNames[i];
            }
            else
            {
                leaderboardPositions[i].text = leaderboardScores[i].ToString("00000") + " - " + leaderboardNames[i];
            }
          
        }
    }
}
