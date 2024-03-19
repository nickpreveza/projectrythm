using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    bool pickedUp = false;

    Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pickedUp)
        {
            transform.position = spawnPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") && pickedUp)
        {
            //EXPLODE BUILDINGS HERE
            Debug.Log("BOOM crash building and so on and so forth");
        }
    }

    public void PickedUp(bool inHand)
    {
        pickedUp = inHand;
    }
}
