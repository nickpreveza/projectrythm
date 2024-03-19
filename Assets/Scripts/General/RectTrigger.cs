using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTrigger : MonoBehaviour
{
    [Range(0,10)] public int sensitivity;
    public bool isTriggered;
    RectTransform rectTransform;
    Image image;
    Camera _Camera;
    void Awake()
    {
        DepthManager.OnTriggerPoints += OnTriggerPoints;
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        _Camera = Camera.main;
    }

    void OnTriggerPoints(List<Vector2> triggerPoints)
    {
        if (!enabled)
        {
            return;
        }

        int count = 0;

        foreach(Vector2 point in triggerPoints)
        {
            //Vector2 flippedY = new Vector2(point.x, _Camera.pixelHeight - point.y);

            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, point))
            {
                count++;
                if (count >= sensitivity)
                {
                    Action();
                    break;
                }
                else
                {
                    ResetState();
                }
            }
        }
    }

    void ResetState()
    {
        isTriggered = false;
        image.color = Color.white;
    }

    void Action()
    {
        isTriggered = true;
        image.color = Color.red;
    }

    private void OnDestroy()
    {
        DepthManager.OnTriggerPoints -= OnTriggerPoints;
    }

}
