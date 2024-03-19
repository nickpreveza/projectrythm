using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RightHandManager : MonoBehaviour
{
    GameObject ui_mouse;

    public GameObject mouseUiPrefab;

    public Sprite idle_hand, good_hand, perfect_hand, miss_hand;

    public Vector3 hand_size;

    bool hit = false, onButton = false;

    SpriteRenderer sr;



    // Start is called before the first frame update
    void Start()
    {
        ui_mouse = Instantiate(mouseUiPrefab, GameObject.Find("Canvas").gameObject.transform);

        sr = GetComponent<SpriteRenderer>();

        sr.size = hand_size;
    }

    private void Update()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.size = hand_size;

        Vector3 wantedPos = Camera.main.WorldToScreenPoint(transform.position);
        if (ui_mouse == null)
        {
            ui_mouse = Instantiate(mouseUiPrefab, GameObject.Find("Canvas").gameObject.transform);

        }
        ui_mouse.transform.position = wantedPos;


        if (hit)
        {
            Invoke("SetIdleHand", 0.5f);

        }
    }

    private void OnDestroy()
    {
        Destroy(ui_mouse);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //ONLY DOES JOYCON STUFF ON COLLISION
        if (collision.gameObject.CompareTag("Target"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onButton = false;   
    }

    void SetIdleHand()
    {
        sr.sprite = idle_hand;
        sr.size = hand_size;
        hit = false;
    }

    public void StartParticles(float time)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;

        main.duration = time;

        ps.Play();
    }

    
}
