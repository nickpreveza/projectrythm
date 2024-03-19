using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] bool mouse_usable = false;
     Vector3 World_Point;
    bool can_trigger_targets = false;
    public Level_Designer levelDesigner;
    // Update is called once per frame
    void Update()
    {

        //tipPos = tip.transform.position;
        //transform.position = tipPos;
        if (GameObject.Find("HandRight") != null)
        {

            transform.position = GameObject.Find("HandRight").transform.position;
        }
        else
        {
            if(mouse_usable)
            {
               
            World_Point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            World_Point.z = 1;
            transform.position = World_Point;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            if (can_trigger_targets)
            {
               
                if(Input.GetKey(KeyCode.Mouse0))
                {
                    
                    if (collision.transform.localScale.x > levelDesigner.scale_of_perfect.x - levelDesigner.allowed_offset.x &&
                        collision.transform.localScale.x < levelDesigner.scale_of_perfect.x + levelDesigner.allowed_offset.x &&
                        collision.transform.localScale.y > levelDesigner.scale_of_perfect.y - levelDesigner.allowed_offset.y &&
                        collision.transform.localScale.y < levelDesigner.scale_of_perfect.y + levelDesigner.allowed_offset.y)
                    {
                        collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        Debug.Log("Perfect!");
                    collision.GetComponent<TargetCode>().Deactivate_Holder_Sprite();
                    collision.GetComponent<TargetCode>().Interacted_With = true;
                        can_trigger_targets = false;
                        return;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        Debug.Log("Too early!");
                    collision.GetComponent<TargetCode>().Deactivate_Holder_Sprite();
                    collision.GetComponent<TargetCode>().Interacted_With = true;
                        can_trigger_targets = false;
                        AudioManager.Instance.Play("normalHit", AudioManager.RandomPitch(0.95f, 1.05f));
                    }
                    /*
                    if (collision.transform.localScale.y < levelDesigner.scale_of_perfect.y - levelDesigner.allowed_offset.y && 
                        collision.transform.localScale.x < Designer.scale_of_perfect.x - Designer.allowed_offset.x)
                    {
                        if (Designer.Interacted_With == false)
                        {
                            
                        }
                    } */
                

                   


               
                }
            }
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.tag == "Cursor")
        {

            if (collision.tag == "Holder")
            {
                can_trigger_targets = true;
            }
        }
    }

}
