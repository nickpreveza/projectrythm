using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NoteGenerator : MonoBehaviour
{
    public GameObject note;

    [Header("Generator Values - Delete all and generate again to apply")]
    public float originOffset;
    public float distanceBetweenThem;
    public float durationOfNotes; //if 0, not affecting the prefab
    public float fallingForce; //if 0, not affecting the prefab
    public float distanceFromAnticipation; //if 0, not affecting the prefab
    public int amount;
    float latestNoteY;

    
    public void GenerateNotes()
    {
        for(int i = 0; i < amount; i++)
        {
            if (transform.childCount > 0)
            {
                latestNoteY = transform.GetChild(transform.childCount - 1).localPosition.y;
                
            }
            else
            {
                latestNoteY = originOffset;
            }

            GameObject newNote = Instantiate(note, this.transform.position, Quaternion.identity);
        
            if (durationOfNotes != 0)
            {
                newNote.GetComponent<FallingBlock>().duration = durationOfNotes; 
            }

            if (fallingForce != 0)
            {
                newNote.GetComponent<FallingBlock>().fallingForce = fallingForce;
            }

            if (distanceFromAnticipation != 0)
            {
                Vector3 positionOfAnticipation = newNote.GetComponent<FallingBlock>().locationOfAnticipation.position;
                positionOfAnticipation.y = distanceFromAnticipation * -1;
                newNote.GetComponent<FallingBlock>().locationOfAnticipation.position = positionOfAnticipation;
            }
            newNote.transform.parent = this.transform;
            newNote.transform.position = Vector3.zero;
            newNote.transform.localPosition = new Vector3(0, latestNoteY + distanceBetweenThem, 0);
        }
    }

    public void DeleteNotes()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

    }
}
