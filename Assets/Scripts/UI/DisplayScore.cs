using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public Text high_score_list;
    GameData data;

    LinkedList<int> highScores;
    int[] arr = new int[10];

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.Load();
        highScores = new LinkedList<int>();
        data = SaveManager.Instance.publicData;

    }

    private void Awake()
    {
        GameManager.OnGameStateChanged += SaveScore;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged += SaveScore;
    }

    void SaveScore(GameState state)
    {
        /*
        if(state == GameState.END || state == GameState.GAMEOVER)
        {
            foreach(int i in highScores)
            {
                if(data.highscore > i)
                {
                    highScores.AddBefore(highScores.Find(i), data.highscore);
                }
                if(data.highscore == i)
                {
                    highScores.AddAfter(highScores.Find(i), data.highscore);
                }
            }

            int temp = 0;

            foreach (int i in highScores)
            {
                arr[temp] = i;
                temp++;
            }

            data.leaderboard0 = arr[0];
            data.leaderboard1 = arr[1];
            data.leaderboard2 = arr[2];
            data.leaderboard3 = arr[3];
            data.leaderboard4 = arr[4];
            data.leaderboard5 = arr[5];
            data.leaderboard6 = arr[6];
            data.leaderboard7 = arr[7];
            data.leaderboard8 = arr[8];
            data.leaderboard9 = arr[9];

            SaveManager.Instance.Save();
        }    */    
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       

    }

}
