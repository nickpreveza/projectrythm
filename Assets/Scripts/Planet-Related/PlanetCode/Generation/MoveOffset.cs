using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffset : MonoBehaviour
{
    public Transform spawn_next;

    // Start is called before the first frame update
    void Start()
    {
          var num = Random.Range(0, 3);

        if(num == 0)
        {
           spawn_next.position += new Vector3(0,2f,0);

        }else if(num == 1)
        {
            spawn_next.position += new Vector3(0, 0, 0);
        }
        else
        {
           spawn_next.position -= new Vector3(0, 1f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DestructionZone")
            Destroy(gameObject);
    }
}
