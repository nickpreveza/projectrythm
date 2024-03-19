using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeInterraction : MonoBehaviour
{
    SimpleBodySourceView sbv;
    GameObject left_hand;
    bool entered = false, picked_up = false;

    Vector3 start_coordinates;

    private void Start()
    {
        if (!GameManager.Instance.isDebug)
        {
            sbv = GameObject.Find("BodyView").GetComponent<SimpleBodySourceView>();
           
        }

        start_coordinates = gameObject.transform.position;

    }
        private void Update()
    {

        if (sbv != null)
        {
            if (sbv.left_hand_closed && entered)
            {

                if (!picked_up)
                {
                    picked_up = true;
                }
                transform.position = left_hand.transform.position;
            }

            if (!sbv.left_hand_closed)
            {
                gameObject.transform.position = start_coordinates;
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LeftHand")
        {
            left_hand = collision.gameObject;

            entered = true;
        }

        if (collision.tag == "House")
        {
            collision.GetComponent<MovingBox>().LaunchBuilding();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sbv != null && !sbv.left_hand_closed)
            entered = false;
    }
}
