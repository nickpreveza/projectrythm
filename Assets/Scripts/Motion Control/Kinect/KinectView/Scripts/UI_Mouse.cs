using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Mouse : MonoBehaviour
{
    private List<Joycon> joycons;

    bool hit = false;

    bool canDoAction;
    float timer = 0.1f;
    float internalTimer;

    // Start is called before the first frame update
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        internalTimer = timer;
        if (joycons != null && joycons.Count > 0)
        {
            joycons[0].SetRumble(0, 0, 0);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if (joycons.Count > 0)
        { 
            Joycon j = joycons[0];

            if (j.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                if (!hit)
                {
                    hit = true;
                    internalTimer = timer;
                }
            }
        }

        if (hit)
        {
            internalTimer -= Time.unscaledDeltaTime;
            if (internalTimer < 0)
            {
                hit = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {

            Button button = collision.GetComponent<Button>();

            if (button != null)
            {
                if (!button.interactable)
                {
                    return;

                }


                KinectButton kinectButton = collision.GetComponent<KinectButton>();
                if (kinectButton != null)
                {
                    kinectButton.SelectWithKinect();
                }


                button.Select();
                if (hit)
                {
                    joycons[0]?.SetRumble(160, 320, 0.6f, 200);
                    button.onClick.Invoke();
                    hit = false;
                }
            }
          
          
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Button")
        {
            KinectButton kinectButton = collision.GetComponent<KinectButton>();
            if (kinectButton != null)
            {
                kinectButton.DeselectWithKinect();
            }

            Button button = collision.GetComponent<Button>();
            if (button != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    void HitOff()
    {
        hit = false;
    }
}
