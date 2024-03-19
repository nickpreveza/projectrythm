using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
    [SerializeField] MultiSourceManager multiSourceManager;
    [SerializeField] DepthManager depthManager;
    [SerializeField] RawImage rawColor;
    [SerializeField] RawImage rawDepth;

    void FixedUpdate()
    {
        rawColor.texture = multiSourceManager.GetColorTexture();
        rawDepth.texture = depthManager.GetDepthTexture();
    }
}
