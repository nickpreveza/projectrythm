using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [Header("UIPanel Universal Settings")]
    public bool fadeIn;
    public bool fadeOut;
    public float fadeDuration = 3f;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public GameObject panelObject;
    [HideInInspector]
    public CanvasGroup canvasGroup;

    [SerializeField] bool fading;

    float targetOpacity;
    float timeElapsed;


    private void Awake()
    {
        panelObject = transform.GetChild(0).gameObject;
        canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
        Disable();
    }

    private void Update()
    {
        if (fading)
        {
            if (!panelObject.activeSelf)
            {
                fading = false;
                return;
            }

            if (timeElapsed < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetOpacity, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                fading = false;
                if (canvasGroup.alpha < 0.8)
                {
                    panelObject.SetActive(false);
                }
                else
                {
                    canvasGroup.alpha = 1;
                }
            }
        }

    }
    public virtual void Activate()
    {
        isActive = true;
        panelObject.SetActive(true);
        if (fadeIn && canvasGroup != null)
        {
            timeElapsed = 0;
            canvasGroup.alpha = 0;
            fading = true;
            targetOpacity = 1f;
            return;
        }
    }

    public virtual void Setup()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
       
    }

    public virtual void Disable()
    {
       
        isActive = false;
        if (fadeOut  && canvasGroup != null)
        {
            timeElapsed = 0;
            fading = true;
            targetOpacity = 0f;
            return;
        }
     
        panelObject.SetActive(false);
    }

}
