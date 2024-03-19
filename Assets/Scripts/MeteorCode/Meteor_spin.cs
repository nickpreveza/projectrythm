using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_spin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, 0.05f, Space.Self);   //Change the last vale, the z value, to change the speed of rotation
    }
}
