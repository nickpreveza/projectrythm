using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public float fallingForce;
    float internalFallingForce;
    public float duration;
    public GameObject below;
    float difference;
    public Transform locationOfAnticipation;
    [SerializeField] Transform locationToPerfect;
    [SerializeField] float gizmosSphereSize;
    // Start is called before the first frame update
    void Start()
    {
        internalFallingForce = 0;
        GameManager.OnGameStateChanged += CheckState;
       
    }

    void OnDrawGizmos()
    {
        if (locationOfAnticipation != null)
        {
            Gizmos.DrawLine(transform.position, locationOfAnticipation.position);
        }
    
    }

    void CheckState(GameState newState)
    {
        if (newState == GameState.GAME)
        {
            internalFallingForce = fallingForce;
        }
        else
        {
            internalFallingForce = 0;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= CheckState;
    }

    // Update is called once per frame
    void Update()
    {
         transform.position -= transform.up * Time.deltaTime * internalFallingForce;
    }
}
