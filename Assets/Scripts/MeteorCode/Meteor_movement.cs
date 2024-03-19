using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_movement : MonoBehaviour
{
    // Start is called before the first frame update
     [SerializeField] GameObject[] left_spawners;
    [SerializeField] GameObject[] right_spawners;
    SpriteRenderer meteoriteImage;
   [SerializeField] Sprite[] Sprites;
    Vector3 velocity;
    public float speed = 1;
    void Start()
    {
        meteoriteImage = GetComponent<SpriteRenderer>();
        meteoriteImage.color = GameManager.Instance.visuals.meteoriteColors[Random.Range(0, GameManager.Instance.visuals.meteoriteColors.Length)];
        meteoriteImage.sprite = Sprites[Random.Range(0, Sprites.Length)];
        left_spawners = GameObject.FindGameObjectsWithTag("LeftSpawner");
        right_spawners = GameObject.FindGameObjectsWithTag("RightSpawner");
        
        if(gameObject.tag == "LeftMeteor")
        {
            velocity = right_spawners[Random.Range(0,  right_spawners.Length)].transform.position - gameObject.transform.position;
        }
        else
        {
            velocity = left_spawners[Random.Range(0, left_spawners.Length)].transform.position - gameObject.transform.position;
        }
        velocity.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += velocity * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.tag == "RightMeteor")
        {
            if(collision.tag == "LeftSpawner")
            {
                GameObject.Destroy(gameObject);
            }
        }
        if (gameObject.tag == "LeftMeteor")
        {
            if (collision.tag == "RightSpawner")
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
