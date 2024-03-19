using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotation_speed;
    float rotZ;


    // Update is called once per frame
    void Update()
    {
        rotZ += Time.deltaTime * rotation_speed;

        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }
}
