using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    public void DoAction(Rigidbody2D source, float fForce)
    {
       // rb.AddForce(fForce, ForceMode2D.Force);
    }
}
