using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Interraction : MonoBehaviour
{
    GameObject left_hand;
    bool entered = false, picked_up = false;
    Vector3 picked_up_coordinates, let_go_coordinates;

    bool hasPlayed = false;

    public float power;

    public bool launch;

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
                            if (!hasPlayed) //this is probably stupid. If it is, please talk to me and we can fix it - Mohammed
                            {
                                AudioManager.Instance.Play("grabMeteor", AudioManager.RandomPitch(0.95f, 1.05f)); //grab sfx
                                hasPlayed = true;
                            }

                            if (!picked_up)
                            {
                                picked_up_coordinates = transform.position;
                                picked_up = true;
                            }

                            transform.position = left_hand.transform.position;
                            
                        }

                        if (b.HandLeftState == Windows.Kinect.HandState.Open && picked_up)
                        {
                            ThrowingObject();
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

        if (collision.tag == "House")
        {
            collision.GetComponent<BuildingInterraction>().LaunchBuilding();
        }

        if (collision.tag == "Planet")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!picked_up)
            if (collision.tag == "LeftHand")
            {
                entered = false;
            }
    }
    void ThrowingObject()
    {
        let_go_coordinates = left_hand.transform.position;


        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<Rigidbody2D>().AddForce((let_go_coordinates - picked_up_coordinates).normalized * power);
        gameObject.transform.parent = null;

        picked_up = false;
        hasPlayed = false;

        AudioManager.Instance.Play("meteorThrow", AudioManager.RandomPitch(0.9f, 1.1f));
    }



}
