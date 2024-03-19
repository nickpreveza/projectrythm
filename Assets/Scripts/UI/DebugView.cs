using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{

    TextMeshProUGUI debugText;

    private void Start()
    {
        debugText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (!GameManager.Instance.isDebug)
        {
            debugText.text = "";
            return;
        }

        if (GameManager.Instance.gameHasStarted)
        {
            debugText.text = "Score :" + ScoreManager.Instance.currentScore;
            debugText.text += "\n Combo :" + ScoreManager.Instance.currentCombo;
            debugText.text += "\n Destruction Points :" + ScoreManager.Instance.currentDestructionPoints;
            debugText.text += "\n Layer:" + ScoreManager.Instance.currentLayer;
            debugText.text += "\n Layer Misses:" + ScoreManager.Instance.currentLayerMisses;
            debugText.text += "\n Total Misses:" + ScoreManager.Instance.totalSessionMisses;
            debugText.text += "\n Total Perfect:" + ScoreManager.Instance.totalSessionPerfectHits;
        }


    }
}
