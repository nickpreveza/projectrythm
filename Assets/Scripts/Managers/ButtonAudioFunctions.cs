using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayHoverSound()
    {
        AudioManager.Instance.Play("hoverButton", AudioManager.RandomPitch(0.95f, 1.05f));
    }
    public void PlayClickSound()
    {
        AudioManager.Instance.Play("clickButton", AudioManager.RandomPitch(0.95f, 1.05f));
    }
}
