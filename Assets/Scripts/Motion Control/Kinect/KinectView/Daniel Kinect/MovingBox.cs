using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    SimpleBodySourceView sbv;
    GameObject left_hand;
    bool entered = false, picked_up = false;
    Vector3 picked_up_coordinates, let_go_coordinates;

    public float power;

    public bool launch;

    bool hasPlayed;

    private void Start()
    { 
        if(GameObject.Find("BodyView") != null)
        {

        sbv = GameObject.Find("BodyView").GetComponent<SimpleBodySourceView>();
        }
    }
    private void Update()
    {
        if (sbv != null)
        {

            if (sbv.left_hand_closed && entered)
            {

                if (!picked_up)
                {
                    picked_up_coordinates = transform.position;
                    picked_up = true;
                }
                transform.position = left_hand.transform.position;
                Debug.DrawLine(picked_up_coordinates, left_hand.transform.position + (left_hand.transform.position).normalized * 10, Color.red, Time.deltaTime);
            }

            if (!sbv.left_hand_closed && picked_up)
            {
                this.gameObject.GetComponent<BuildingObject>().DestroyStructure();
            }

            if (launch)
            {
                //LaunchBuilding();
               // launch = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LeftHand")
        {
            left_hand = collision.gameObject;

            entered = true;

            AudioManager.Instance.Play("grab", AudioManager.RandomPitch(0.95f, 1.05f)); //grab sfx
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sbv != null)
        {

            if (!sbv.left_hand_closed)
                entered = false;
        }
    }

    void ThrowingObject()
    {
        let_go_coordinates = left_hand.transform.position;


        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce((let_go_coordinates).normalized * power);
        gameObject.transform.parent = null;

        picked_up = false;

        //sound effect for throwing building?
    }

    public void LaunchBuilding()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * power);
        gameObject.transform.parent = null;

        //sound effect for throwing building?
    }
}
