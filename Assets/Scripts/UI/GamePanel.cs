using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePanel : UIPanel
{
    public TextMeshProUGUI hitStatus;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;

    [SerializeField] Image feedbackImage;

    [SerializeField] Sprite feedbackEarly;
    [SerializeField] Sprite feedbackPerfect;
    [SerializeField] Sprite feedbackLate;
    [SerializeField] Image gaugeFill;
    Color transparent;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateScoreCombo(0, 1);
        feedbackImage.color = transparent;
        GameManager.OnPointHit += OnPointHit;
    }

    private void Update()
    {
        if (GameManager.Instance.gameHasStarted)
        {
            gaugeFill.fillAmount = ScoreManager.Instance.gaugePercentage;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPointHit -= OnPointHit;
    }

    void OnPointHit(TargetState pointState, AnticipationPoint point)
    {
        StopAllCoroutines();

        switch (pointState)
        {
            case TargetState.EARLY:
                feedbackImage.sprite = feedbackEarly;
                break;
            case TargetState.PERFECT:
                feedbackImage.sprite = feedbackPerfect;
                break;
            case TargetState.LATE:
                feedbackImage.sprite = feedbackLate;
                break;
        }

        feedbackImage.color = Color.white;
        StartCoroutine(HideFeedbackImage(0.5f));
    }

    IEnumerator HideFeedbackImage(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        feedbackImage.color = transparent;
    }

    public void UpdateScoreCombo(float score, float combo)
    {
        if (combo < 1)
        {
            combo = 1;
        }
        scoreText.text = score.ToString("00000");
        comboText.text = "x" + combo.ToString();
    }

    public void Pause()
    {
        GameManager.Instance.SetPause = true;
    }

    public override void Setup()
    {
        base.Setup();
    }

    public override void Activate()
    {
        base.Activate();

    }

    public override void Disable()
    {
        base.Disable();
    }
}
