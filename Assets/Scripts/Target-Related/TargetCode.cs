using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCode : MonoBehaviour
{
    public float duration;
    Vector3 Scale_Increase;
    bool isActivated;
   public bool Interacted_With = false;
    GameObject baseBlock;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated)
        {
            return;
        }

        if (duration != 0)
        {

            Scale_Increase = ( Level_Designer.Instance.scale_of_perfect -  Level_Designer.Instance.scale_of_min) / duration * Time.deltaTime;
        }
        transform.localScale += Scale_Increase;
        if (transform.localScale.x >  Level_Designer.Instance.scale_of_perfect.x -  Level_Designer.Instance.allowed_offset.x &&
            transform.localScale.x <  Level_Designer.Instance.scale_of_perfect.x +  Level_Designer.Instance.allowed_offset.x &&
            transform.localScale.y >  Level_Designer.Instance.scale_of_perfect.y -  Level_Designer.Instance.allowed_offset.y &&
            transform.localScale.y <  Level_Designer.Instance.scale_of_perfect.y +  Level_Designer.Instance.allowed_offset.y)
        {
            GetComponent<SpriteRenderer>().sprite =  Level_Designer.Instance.perfect;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite =  Level_Designer.Instance.normal;
        }

        if (transform.localScale.x >=  Level_Designer.Instance.scale_of_perfect.x && transform.localScale.y >=  Level_Designer.Instance.scale_of_perfect.y)
        {
            if ( Interacted_With == false)
            {
                //Score_Manager.Instance.Miss();
            }
            baseBlock.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject);
        }
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
        isActivated = true;
    }

    public void SetHolder(GameObject holder)
    {
        baseBlock = holder;
    }

    public void Deactivate_Holder_Sprite()
    {
        baseBlock.GetComponent<SpriteRenderer>().enabled = false;
    }
}
