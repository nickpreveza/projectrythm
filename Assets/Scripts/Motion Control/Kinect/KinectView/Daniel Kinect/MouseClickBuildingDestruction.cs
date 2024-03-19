using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickBuildingDestruction : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.GetComponent<ParticleSystem>().Play();

        if(gameObject.GetComponent<ParticleSystem>().isPlaying)
        Destroy(gameObject);
    }


}
