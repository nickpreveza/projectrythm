using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldShake : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LeftMeteor") || collision.CompareTag("RightMeteor"))
        {
            CameraShake.instance.Shake(1f, 10f,2f);

            Destroy(collision.gameObject);
        }
    }
}
