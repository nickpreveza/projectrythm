using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    // Start is called before the first frame updat
    public float off_set;
    float height_per_point;
    void Start()
    {
      height_per_point = -off_set/ScoreManager.Instance.gaugeMaxDestructionPoints;
        
    }

    // Update is called once per frame
    void Update()
    {

       transform.position = new Vector3(transform.position.x, off_set + ScoreManager.Instance.currentDestructionPoints*height_per_point , 0);

    }


}
