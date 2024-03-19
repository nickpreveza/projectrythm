using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConLeft : MonoBehaviour
{
    private List<Joycon> joycons;
    bool hit = false;

    public float power;
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
            Joycon j = joycons[1];

            
            if (j.GetButtonUp(Joycon.Button.DPAD_UP))
            {
                hit = false;
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (joycons.Count > 0)
        {
            Joycon j = joycons[1];
            if (collision.CompareTag("Building"))
            {
                if (j.GetButtonDown(Joycon.Button.DPAD_UP))
                {
                    j.SetRumble(160, 320, 0.6f, 200);

                    collision.gameObject.transform.position = transform.position;
                }
            }
        }
    }

    void ThrowObject(GameObject obj)
    {
        obj.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
        obj.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        foreach (var gameObject in obj.GetComponentsInChildren<Rigidbody2D>())
        {
            gameObject.isKinematic = false;
            gameObject.gravityScale = 0;

            Vector3 direction = (transform.position - obj.transform.position).normalized;

            if (direction == new Vector3())
            {
                obj.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.down * (power * 1000));
                return;
            }


            gameObject.AddForce(direction * (power * 1000));


        }
        ScoreManager.Instance.currentScore += 3;
    }

    void HitOff()
    {
        hit = false;
    }
}
