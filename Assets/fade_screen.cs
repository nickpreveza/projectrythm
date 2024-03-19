using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fade_screen : MonoBehaviour
{
    public float increase_rate;
    public float decrease_rate;
    public MenuPanel menu_panel;
    bool used = false;
    bool running;
    int game_number;
    // Start is called before the first frame update
    void Start()
    {
        used = false;
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(running)
        {

        if(used == false)
        {

        gameObject.GetComponent<CanvasGroup>().alpha += increase_rate;
            if(gameObject.GetComponent<CanvasGroup>().alpha >= 1)
            {
                used = true;
                menu_panel.StartGame(game_number);
                    
            }
        }
        else
        {
                if (GameManager.Instance.State == GameState.GAME)
                {

                    gameObject.GetComponent<CanvasGroup>().alpha -= decrease_rate;
                    if (gameObject.GetComponent<CanvasGroup>().alpha <= 0)
                    {
                        used = false;
                        running = false;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void choose_game(int number)
    {
        game_number = number;
    }
    

}
