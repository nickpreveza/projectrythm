using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeftHand : MonoBehaviour
{
    private List<Joycon> joycons;

    GameObject trackedObject, slowHandPos;

    bool hit = false, handFull = false;

    Vector3 releseCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        joycons = JoyconManager.Instance.j;

        handFull = false;
        slowHandPos = new GameObject("LeftHandLerpPos");
    }

    // Update is called once per frame
    void Update()
    {
        slowHandPos.transform.position = Vector2.Lerp(slowHandPos.transform.position, transform.position, Time.deltaTime);

        if (joycons.Count > 0)
        {
            Joycon j = joycons[1];

            if(handFull && j.GetButton(Joycon.Button.DPAD_UP))
            {

            }

            if (j.GetButtonUp(Joycon.Button.DPAD_UP))
            {
                Debug.Log("Dpad up");
                releseCoordinates = transform.position;
                handFull = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") || collision.CompareTag("LeftMeteor") || collision.CompareTag("RightMeteor"))
        {

            if (joycons.Count > 0)
            {
                Joycon j = joycons[1];

                if (j.GetButtonDown(Joycon.Button.DPAD_UP) && ScoreManager.Instance.currentDestructionPoints > 0)
                {
                    if (collision.CompareTag("Building"))
                    {

                        ScoreManager.Instance.currentDestructionPoints -= 2;
                    }
                    else
                    {
                        ScoreManager.Instance.currentDestructionPoints -= 1;
                    }

                    trackedObject = collision.gameObject;
                    trackedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    trackedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    trackedObject.transform.parent = null;

                    handFull = true;
                }
            }
        }
    }
}
