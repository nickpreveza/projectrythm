using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// DEPRECATED
/// </summary>
public class BaseBlocks : MonoBehaviour
{
    [SerializeField] bool metronome;
    [SerializeField] bool mute;
    [SerializeField] bool randomGeneration;
    [SerializeField] AnticipationPoint targetPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!metronome)
        {

            if (collision.gameObject.CompareTag("AnticipationTrigger"))
            {
                if (!randomGeneration)
                {
                    GameManager.Instance.pointHandler.SpawnAnticipationCircle(targetPoint);
                }

            }
            else if (collision.gameObject.CompareTag("Blocks"))
            {
                GameManager.Instance.pointHandler.SpawnPoint(targetPoint);
                Destroy(collision.gameObject);

            }
            else if (collision.gameObject.CompareTag("End"))
            {
                GameManager.Instance.EndRound();

            }

        }
        else
        {
            if (collision.gameObject.CompareTag("Blocks"))
            {
                if (!mute)
                {
                    AudioManager.Instance.Play("Metronome");
                }
               

                return;

            }
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position+new Vector3(0,1000,0));
    }
}
