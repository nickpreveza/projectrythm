using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<GameObject> listOfCanvasPanels = new List<GameObject>();
    bool hasPlayed = false;

    private void Start()
    {
        
    }

    void Update()
    {
        if(listOfCanvasPanels[0].active) //if main menu is open
        {
            if (!hasPlayed)
            {
                AudioManager.Instance.Play("mainMenuTrack");
                hasPlayed = true;
            }
        }
        if(listOfCanvasPanels[1].active)
        {
            AudioManager.Instance.Stop("mainMenuTrack");
        }
    }
}
