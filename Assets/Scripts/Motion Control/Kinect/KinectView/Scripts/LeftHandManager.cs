using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandManager : MonoBehaviour
{
    public static bool handFull;

    GameObject trackedObject;

    Windows.Kinect.Body[] body;

    public Sprite openHand, closedHand;
    

    bool dontDestroy = false;


    private void Start()
    {
        handFull = false;
    }

    private void Update()
    {
        body = BodySourceManager.Instance.GetData();
        foreach (var b in body)
        {
            if (b.IsTracked)
            {
                if (b.HandLeftState == Windows.Kinect.HandState.Closed)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = closedHand;
                }
                if (b.HandLeftState == Windows.Kinect.HandState.Open)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = openHand;
                    if (dontDestroy)
                    {
                        dontDestroy = false;
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            foreach (var b in body)
            {
                if (b.IsTracked)
                {
                    if (b.HandLeftState == Windows.Kinect.HandState.Closed && !dontDestroy)
                    {
                        if (collision.CompareTag("Building"))
                        {
                            // AudioManager.Instance.Play("buildingCrumble", AudioManager.RandomPitch(0.7f, 1.3f), AudioManager.crumblingSoundVolume);
                            //collision.gameObject.GetComponent<BuildingObject>().DestroyStructure();
                            return;
                        }
                    }
                }
            }
        }
        if (collision.CompareTag("LeftMeteor") || collision.CompareTag("RightMeteor"))
        {
            foreach (var b in body)
            {
                if (b.IsTracked)
                {
                    if (b.HandLeftState == Windows.Kinect.HandState.Closed)
                    {
                            collision.GetComponent<MeteorManager>().ThrowMeteor(transform);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building"))
        {
            foreach(var b in body)
            {
                if (b.IsTracked)
                {
                    if(b.HandLeftState == Windows.Kinect.HandState.Closed)
                    {
                        dontDestroy = true;
                    }
                }
            }
        }
    }  
}
