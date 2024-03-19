using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPlanet : MonoBehaviour
{
    public SimpleBodySourceView sbv;

    public bool left;
    public float rotation_speed;
    public string next_scene;

    bool entered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(left)
        transform.eulerAngles += Vector3.forward*(rotation_speed/10);
        else
            transform.eulerAngles -= Vector3.forward*(rotation_speed/10);


        if(entered && sbv.left_hand_closed)
        {
            SceneManager.LoadScene(next_scene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LeftHand")
        {
            Debug.Log("ENTERED");
            entered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
    }
}
