using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGeneration : MonoBehaviour
{

    public GameObject[] houses;
    public Transform spawn_point;

    float duration = 1f, timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawn_point.SetParent(null, true);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= duration)
        {
            SpawnBuilding();

            duration = Random.Range(1, 5);

            timer = 0f;
        }
    }

    void SpawnBuilding()
    {
        int random_house = Random.Range(0, houses.Length);

        var obj = Instantiate(houses[random_house], spawn_point.transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.Rotate(0, 0, -110);

    }
}
