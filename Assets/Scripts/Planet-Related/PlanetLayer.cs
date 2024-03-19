using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLayer : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    float rotZ;

    public void SetSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotZ +=  rotationSpeed * GameManager.Instance.rotationSpeedMultiplier * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }


}
