using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoint : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float force = 10;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            other.gameObject.GetComponent<TestTarget>().DoAction(rb, force);
        }
    }
}
