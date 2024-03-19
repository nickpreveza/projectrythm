using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInterraction : MonoBehaviour
{
    SimpleBodySourceView sbv;
    GameObject left_hand;
    bool entered = false, picked_up = false;
    Vector3 picked_up_coordinates, let_go_coordinates;

    public float power;

    public bool launch;

    bool hasPlayed = false;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (BodySourceManager.Instance != null)
        {
            Windows.Kinect.Body[] body = BodySourceManager.Instance.GetData();
            if (body != null)
            {
                foreach (var b in body)
                {
                    if (b.IsTracked)
                    {
                        if (b.HandLeftState == Windows.Kinect.HandState.Closed && entered)
                        {
                           
                            if (!picked_up)
                            {
                                picked_up_coordinates = transform.position;
                                picked_up = true;
                            }
                            /*
                           transform.position = left_hand.transform.position;
                           if (!hasPlayed) //this is probably stupid. If it is, please talk to me and we can fix it - Mohammed
                           {
                               AudioManager.Instance.Play("grabBuilding", AudioManager.RandomPitch(0.95f, 1.05f)); //grab sfx
                               hasPlayed = true;
                           } */
                        }

                        if (b.HandLeftState == Windows.Kinect.HandState.Open && picked_up)
                        {
                            ThrowingObject(new Vector3(), new Vector3());
                        }
                    }
                }
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

        if(collision.tag == "Planet")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!picked_up)
            if(collision.tag == "LeftHand")
            {
                entered = false;
                hasPlayed = false;
            }
    }
    void ThrowingObject(Vector3 pickup_pos, Vector3 end_pos)
    {
        let_go_coordinates = left_hand.transform.position;


        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce((let_go_coordinates - picked_up_coordinates).normalized * power);
        gameObject.transform.parent = null;

        picked_up = false;

        foreach(var gameObject in GetComponentsInChildren<Rigidbody2D>())
        {
            gameObject.isKinematic = false;
            gameObject.gravityScale = 0;

            gameObject.AddForce((let_go_coordinates - picked_up_coordinates).normalized * power);

            
        }

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
