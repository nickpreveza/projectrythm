using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRightHand : MonoBehaviour
{
    private List<Joycon> joycons;

    int jc_ind = 0;
    bool hit = false;
    int next_screen = 0;

    [SerializeField] float rumbleTimer = 0.1f;
    float internalTimer;


    // Start is called before the first frame update
    void Start()
    {
        joycons = JoyconManager.Instance.j;
    }

    // Update is called once per frame
    void Update()
    {
        if (joycons.Count > 0)
        {
            Joycon j = joycons[0];

            if (j.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                j.SetRumble(160, 320, 0.6f, 200);
                internalTimer = rumbleTimer;
                hit = true;
            }
        }

        if (hit)
        {
            internalTimer -= Time.deltaTime;
            if (internalTimer < 0)
            {
                HitOff();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            KinectButton kinectButton = collision.GetComponent<KinectButton>();
            if (kinectButton != null)
            {

            }

            Button button = collision.GetComponent<Button>();
            if (button != null)
            {
                if (hit)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Button")
        {
            //gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    void HitOff()
    {
        hit = false;
    }
}
