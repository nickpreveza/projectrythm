using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    //Stores Calculated screen bounds
    Vector2 bottomLeft = Vector2.zero;
    Vector2 topRight = Vector2.zero;

    private void Awake()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.farClipPlane));
    }
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(0,0,Camera.main.farClipPlane)), 0.5f);
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.farClipPlane)), 0.5f);
    }

    public Vector3 GetPlanePosition()
    {
        float targetX = Random.Range(bottomLeft.x, topRight.x);
        float targetY = Random.Range(bottomLeft.y, topRight.y);

        return new Vector3(targetX, targetY, 0);
    }

    void Update()
    {
        
    }
}
