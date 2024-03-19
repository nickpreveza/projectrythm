using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Designer : MonoBehaviour
{
    public static Level_Designer Instance;

    public Sprite normal;
    public Sprite perfect;
    public float Duration_until_start_of_song;
    public Vector3 scale_of_max;
    public Vector3 scale_of_perfect;
    public Vector3 scale_of_min;
    public Vector3 allowed_offset;
    public float radius_of_detection;
    public Vector2 size_of_detection;
    public float speed_of_controller;
    public bool indicator_for_upcoming = false;

    [Header("Include all targets in pattern 0")] 

    public Pattern[] pattern;

    [Header("Make sure that timestamps go in ascending order of time.")]

    public Data[] timestamps;
    public bool Interacted_With = false;
    public int Current_Index = 0;
    [SerializeField] float Duration_Until_Perfect = 0.5f;
    [SerializeField] int Pattern_Number = 0;
    [SerializeField] float Stopwatch = 0;
    [SerializeField] Vector3 Scale_Increase;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        /*
        for (int i = 0; i < pattern[0].targets.Length; i++)
        {
            if (pattern[0].targets[i].GetComponent<CircleCollider2D>() != null)
            {

                pattern[0].targets[i].GetComponent<CircleCollider2D>().radius = radius_of_detection;
            }
            if (pattern[0].targets[i].GetComponent<BoxCollider2D>() != null)
            {

                pattern[0].targets[i].GetComponent<BoxCollider2D>().size = size_of_detection;
                pattern[0].targets[i].GetComponent<SpriteRenderer>().sprite = normal;
            }

        }*/
    }

    void Update()
    {
        //Determines the scale inscrease based on duration chosen
        if (Duration_Until_Perfect != 0)
        {

            Scale_Increase = (scale_of_perfect - scale_of_min) / Duration_Until_Perfect * Time.deltaTime;
        }

        Stopwatch += Time.deltaTime;


        for (int i = 0; i < timestamps.Length; i++)
        {
            if (timestamps[i].Time_Stamp_Used == false)
            {
                if (timestamps[i].Time_Stamp < Stopwatch)
                {
                   
                    timestamps[i].Time_Stamp_Used = true;
                    Pattern_Number = timestamps[i].Pattern;
                    Duration_Until_Perfect = timestamps[i].targets_Duration;

                    if (indicator_for_upcoming)
                    {

                        for (int s = 0; s < pattern[0].targets.Length; s++)
                        {

                            pattern[0].targets[s].GetComponentInParent<SpriteRenderer>().enabled = false;
                            pattern[0].targets[s].gameObject.SetActive(false);
                            pattern[0].targets[pattern[0].targets.Length-1].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }



        //Waits before starting everything
        if (Stopwatch > Duration_until_start_of_song)
        {
            for(int x = 0; x<pattern.Length; x++)
            {

                //If the first pattern is selected
                if (Pattern_Number == x)
                {

                    //Loops the 2 Pattern positions
                    if (Current_Index >= pattern[x].targets.Length)
                    {
                        Current_Index = 0;
                    }

                    //If inactive, reset the size and activate. also manages the signs of next targets
                    if (pattern[x].targets[Current_Index].activeInHierarchy == false)
                    {
                        if (indicator_for_upcoming)
                        {
                            pattern[x].targets[Current_Index].GetComponentInParent<SpriteRenderer>().enabled = true;
                        }
                        pattern[x].targets[Current_Index].transform.localScale = scale_of_min;
                        pattern[x].targets[Current_Index].SetActive(true);
                        Interacted_With = false;
                        pattern[x].targets[Current_Index].GetComponent<SpriteRenderer>().enabled = true;

                        if (indicator_for_upcoming)
                        {

                            if (Current_Index + 1 < pattern[x].targets.Length)
                            {
                                pattern[x].targets[Current_Index + 1].GetComponentInParent<SpriteRenderer>().enabled = true;
                            }
                            else
                            {
                                pattern[x].targets[0].GetComponentInParent<SpriteRenderer>().enabled = true;
                            }    
                           
                        }
                    }

                

                    //Slowly increase the scale
                    if(pattern[x].targets[Current_Index].transform.localScale.x>=1)
                    {
                        Scale_Increase.x = 0;
                    }
                    if (pattern[x].targets[Current_Index].transform.localScale.y >= 1)
                    {
                        Scale_Increase.y = 0;
                    }
                    pattern[x].targets[Current_Index].transform.localScale += Scale_Increase;

                    if (pattern[x].targets[Current_Index].transform.localScale.x > scale_of_perfect.x - allowed_offset.x && pattern[x].targets[Current_Index].transform.localScale.x < scale_of_perfect.x + allowed_offset.x && pattern[x].targets[Current_Index].transform.localScale.y > scale_of_perfect.y - allowed_offset.y && pattern[x].targets[Current_Index].transform.localScale.y < scale_of_perfect.y + allowed_offset.y)
                    {
                        pattern[x].targets[Current_Index].GetComponent<SpriteRenderer>().sprite = perfect;
                    }
                    else
                    {
                        pattern[x].targets[Current_Index].GetComponent<SpriteRenderer>().sprite = normal;
                    }

                    //If scale equals or is greater than max allowed, turn off and increase index
                    if (pattern[x].targets[Current_Index].transform.localScale.x >= scale_of_perfect.x && pattern[x].targets[Current_Index].transform.localScale.y >= scale_of_perfect.y)
                    {
                        if (Interacted_With == false)
                        {
                            //Score_Manager.Instance.Miss();
                        }
                        pattern[x].targets[Current_Index].SetActive(false);
                        pattern[x].targets[Current_Index].GetComponentInParent<SpriteRenderer>().enabled = false;
                        Current_Index++;
                    }
                }
            }
        }
    }

  

    [System.Serializable]
    public class Data
    {
        public float Time_Stamp;
        public float targets_Duration;
        public int Pattern;

        [HideInInspector]
        public bool Time_Stamp_Used = false;
    }

    [System.Serializable]
    public class Pattern
    {
        public GameObject[] targets;
    }
}
