using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBuilding : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Plant") || collision.CompareTag("House") || collision.tag == "LeftMeteor" || collision.tag == "RightMeteor")
        {
            Destroy(collision.gameObject);
        }
    }
}
