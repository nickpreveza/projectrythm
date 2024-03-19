using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public void ToDestruct(float time)
    {
        StartCoroutine(SelfDestructEnum(time));
    }


    IEnumerator SelfDestructEnum(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
