using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandler : MonoBehaviour
{
    // Start is called before the first frame update
     public GameObject godCharacter;
    void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public void DisableGod()
    {
        godCharacter.SetActive(false);
    }

    public void EnableGod()
    {
        godCharacter.SetActive(true);
    }

    public void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.GAME:
                godCharacter.SetActive(false);
                break;
            case GameState.END:
            case GameState.GAMEOVER:
                godCharacter.SetActive(false);
                break;
            case GameState.START:
                godCharacter.SetActive(true);
                break;
        }
    }

    
}
