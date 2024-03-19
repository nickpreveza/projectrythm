using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Spawning : MonoBehaviour
{
    [SerializeField]  GameObject meteor;

    private void Start()
    {
        GameManager.Instance.AddMeteorSpawner(this);
    }
    public void SpawnMeteor()
    {
        GameObject.Instantiate(meteor, gameObject.transform.position, gameObject.transform.rotation);
    }

 
}
