using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnticipationPoint : MonoBehaviour
{
    CanvasGroup anticipationGroup;

    bool lerpAnticipationImage;
    float targetOpacity;
    float timeElapsed;
    [SerializeField] float fadeDuration = 0.5f;
    void Start()
    {
        anticipationGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
    }

    public void HideAnticipationImage()
    {
        targetOpacity = 0;
        timeElapsed = 0;
        lerpAnticipationImage = true;
    }

    public void ShowAnticipationImage()
    {
        targetOpacity = 1;
        timeElapsed = 0;
        lerpAnticipationImage = true;
        //anticipationGroup.alpha = 1;
    }

    private void Update()
    {
        if (lerpAnticipationImage)
        {
            if (timeElapsed < fadeDuration)
            {
                anticipationGroup.alpha = Mathf.Lerp(anticipationGroup.alpha, targetOpacity, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                anticipationGroup.alpha = targetOpacity;
                lerpAnticipationImage = false;

            }
        }

    }

    public void SpawnPoint(float duration)
    {
        GameObject obj = Instantiate(GameManager.Instance.pointPrefabUI, this.transform);
        obj.GetComponent<UIPoint>().pointParent = this;
        obj.GetComponent<UIPoint>().recreateOnPosition = false;
        obj.GetComponent<UIPoint>().duration = duration;
        obj.GetComponent<UIPoint>().isGrowing = true;
    }

    public void SpawnPoint()
    {
        GameObject obj = Instantiate(GameManager.Instance.pointPrefabUI, this.transform);
        obj.GetComponent<UIPoint>().pointParent = this;
        obj.GetComponent<UIPoint>().recreateOnPosition = false;
        obj.GetComponent<UIPoint>().isGrowing = true;
    }

}
