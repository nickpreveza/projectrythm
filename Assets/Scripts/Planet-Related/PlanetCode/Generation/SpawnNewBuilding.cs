using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewBuilding : MonoBehaviour
{
    BuildingGeneration bg;
    GameObject front_house;

    private void Awake()
    {
        bg = GetComponentInParent<BuildingGeneration>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //front_house = bg.getFrontHouse(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "BuildingSpawn")
        {
            //bg.SpawnBuilding();
        }
    }
}
