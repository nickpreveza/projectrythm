using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPoint : MonoBehaviour
{
    public AnticipationPoint pointParent;
    TargetState currentState = TargetState.VERYEARLY;
    [SerializeField] GameObject pointPrefab;
    public float duration;
    public bool isTutorial;
    Button button;
    RectTransform thisRect;
    [SerializeField] RectTransform targetRect;
    Image targetImage;

    public bool recreateOnPosition;
    public bool isGrowing;
    [SerializeField] Vector3 startingTargetScale = new Vector3(0.1f, 0.1f, 1);
    [SerializeField] Vector3 goalTargetScale = new Vector3(1, 1, 1);
    float timeElapsed;
    Vector3 modifiedScale;

    [Range(0, 1)]
    [SerializeField] float earlyState;
    [Range(0, 1)]
    [SerializeField] float perfectState;
    [Range(0, 1)]
    [SerializeField] float lateState;

    [SerializeField] CanvasGroup canvasGroup;
    bool fading;
    float targetOpacity;
    float timeElapsedFade;
    [SerializeField] float fadeDuration = 1f;
    bool destroyOnEnd;
    bool hasBeenInteracted;
    private void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void FadeIn()
    {
        canvasGroup.alpha = 0;
        targetOpacity = 1;
        timeElapsedFade = 0;
        fading = true;
    }

    void FadeOut(bool shouldDestroy)
    {
        canvasGroup.alpha =1;
        targetOpacity = 0;
        timeElapsedFade = 0;
        fading = true;
        destroyOnEnd = shouldDestroy;
    }


    void Start()
    {
        currentState = TargetState.VERYEARLY;
        targetRect.localScale = startingTargetScale;
        targetImage = targetRect.GetComponent<Image>();
        targetImage.sprite = GameManager.Instance.visuals.earlyState;
        button.interactable = false;
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrowing)
        {
            if (timeElapsed < duration)
            {
                modifiedScale = targetRect.localScale;
                modifiedScale.x = Mathf.Lerp(startingTargetScale.x, goalTargetScale.x, timeElapsed / duration);
                modifiedScale.y = Mathf.Lerp(startingTargetScale.y, goalTargetScale.y, timeElapsed / duration);

                targetRect.localScale = modifiedScale;

                CheckScaleState();
                timeElapsed += Time.deltaTime;
            }
            else
            {
                isGrowing = false;
                EndOfLife(true);
               
            }
        }

        if (fading)
        {
            if (timeElapsedFade < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetOpacity, timeElapsedFade / fadeDuration);
                timeElapsedFade += Time.deltaTime;
            }
            else
            {
                fading = false;
                button.interactable = true;
                if (destroyOnEnd)
                {
                    Destroy(this.gameObject);
                }
            }
        }

    }

    public void CheckScaleState()
    {
        float currentPercentage = targetRect.localScale.x / goalTargetScale.x;
        if (currentPercentage < earlyState)
        {
            button.interactable = false;
        }
        else if (currentPercentage >= earlyState && currentPercentage < perfectState)
        {
            if (!hasBeenInteracted)
            {
                button.interactable = true;
            }
          
            targetImage.sprite = GameManager.Instance.visuals.earlyState;
            currentState = TargetState.EARLY;
        }
        else if (currentPercentage >= perfectState && currentPercentage < lateState)
        {
            if (!hasBeenInteracted)
            {
                button.interactable = true;
            }
            targetImage.sprite = GameManager.Instance.visuals.perfectState;
            currentState = TargetState.PERFECT;
        }
        else if (currentPercentage >= lateState)
        {
            hasBeenInteracted = true;
            button.interactable = false;
            targetImage.sprite = GameManager.Instance.visuals.lateState ;
            currentState = TargetState.LATE;
        }
    }

    public void Interact()
    {
        hasBeenInteracted = true;
        button.interactable = false;
        isGrowing = false;
        CheckScaleState();

        switch (currentState)
        {
            case TargetState.EARLY:
                Debug.Log("State: Early");
                AudioManager.Instance.Play("normalHit", AudioManager.RandomPitch(0.9f, 1.1f), AudioManager.alrightHitSoundVolume);
                targetImage.sprite = GameManager.Instance.visuals.earlyState_interacted;
                break;
            case TargetState.PERFECT:
                Debug.Log("State: Perfect");
                AudioManager.Instance.Play("perfectHit", AudioManager.RandomPitch(0.9f, 1.1f), AudioManager.perfectHitSoundVolume);
                targetImage.sprite = GameManager.Instance.visuals.perfectState_interacted;
                break;
            case TargetState.LATE:
                Debug.Log("State: Late");
                AudioManager.Instance.Play("normalHit", AudioManager.RandomPitch(0.9f, 1.1f), AudioManager.alrightHitSoundVolume);

                break;

        }


        GameManager.Instance.PointHit(currentState, pointParent);
        EndOfLife(false);
    }

    public void EndOfLife(bool missed)
    {
        if (recreateOnPosition)
        {
            isGrowing = false;
            GameObject obj = Instantiate(GameManager.Instance.pointPrefabUI, transform.parent.transform);
            obj.GetComponent<UIPoint>().recreateOnPosition = true;
            obj.GetComponent<UIPoint>().duration = duration;
            obj.GetComponent<UIPoint>().isGrowing = true;
            Destroy(this.gameObject);
        }
        else
        {
            if (missed)
            {
                GameManager.Instance.TryRegisterMiss(pointParent);
            }

            FadeOut(true);
          
        }
      
    }
}



public enum TargetState
{
    VERYEARLY,
    EARLY,
    PERFECT,
    LATE
}
