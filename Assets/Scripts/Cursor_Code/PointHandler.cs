using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointHandler : MonoBehaviour
{
    public bool randomGeneration;
    [SerializeField] List<AnticipationPoint> anticipationPointsPool = new List<AnticipationPoint>();
    [SerializeField] List<AnticipationPoint> occupiedPointsPool = new List<AnticipationPoint>();
    public AnticipationPoint selectedPoint;
    AnticipationPoint nextPoint;
    bool firstTime;
    public float durationOfPoints;

    private void Awake()
    {
        foreach (AnticipationPoint point in anticipationPointsPool)
        {
            point.HideAnticipationImage();
        }
    }

    private void Start()
    {
        GameManager.OnPointHit += OnPointHit;
        GameManager.OnPointMissed += OnPointMissed;

        if (!firstTime)
        {
            firstTime = true;
            foreach (AnticipationPoint point in anticipationPointsPool)
            {
                point.HideAnticipationImage();
            }
        }


    }

    public void OnPointMissed(AnticipationPoint pointParent)
    {
        if (!GameManager.Instance.gameHasStarted)
        {
            return;
        }
        pointParent.HideAnticipationImage();
        if (pointParent != null)
        {
            
            //occupiedPointsPool.Remove(pointParent);
            //anticipationPointsPool.Add(pointParent);

            if (selectedPoint == pointParent)
            {
                selectedPoint = null;
            }
        }
    }

    public void OnPointHit(TargetState pointState, AnticipationPoint pointParent)
    {
        if (!GameManager.Instance.gameHasStarted)
        {
            return;
        }

        pointParent.HideAnticipationImage();

        if (pointParent != null)
        {
            //occupiedPointsPool.Remove(pointParent);
           // anticipationPointsPool.Add(pointParent);

            if (selectedPoint == pointParent)
            {
                selectedPoint = null;
            }
        }

        selectedPoint = null;
    }

    public void SpawnAnticipationCircle(AnticipationPoint newPoint)
    {
        newPoint.ShowAnticipationImage();
    }


    public void SpawnPoint(AnticipationPoint targetPoint = null)
    {
        if (!firstTime)
        {
            firstTime = true;
            foreach(AnticipationPoint point in anticipationPointsPool)
            {
                point.HideAnticipationImage();
            }
        }

        if (randomGeneration)
        {

            SpawnRandomPoint();
        }
        else
        {
            if (targetPoint != null)
            {
                occupiedPointsPool.Add(selectedPoint);
                anticipationPointsPool.Remove(selectedPoint);
                selectedPoint = targetPoint;
                selectedPoint.SpawnPoint();
            }
        }
    }
    
    public void SpawnRandomPoint()
    {
        if (selectedPoint == null && nextPoint == null)
        {
            int randomPoint = Random.Range(0, anticipationPointsPool.Count);
            selectedPoint = anticipationPointsPool[randomPoint];
            occupiedPointsPool.Add(selectedPoint);
            anticipationPointsPool.Remove(selectedPoint);

            selectedPoint.ShowAnticipationImage();
            selectedPoint.SpawnPoint(durationOfPoints);

            int randomPointNext = Random.Range(0, anticipationPointsPool.Count);
            nextPoint = anticipationPointsPool[randomPointNext];
            occupiedPointsPool.Add(nextPoint);
            anticipationPointsPool.Remove(nextPoint);

            nextPoint.ShowAnticipationImage();
        }

        else if (selectedPoint == null && nextPoint != null)
        {
            selectedPoint = nextPoint;
            selectedPoint.ShowAnticipationImage();
            selectedPoint.SpawnPoint(durationOfPoints);

            int randomPoint = Random.Range(0, anticipationPointsPool.Count);
            nextPoint = anticipationPointsPool[randomPoint];
            occupiedPointsPool.Add(nextPoint);
            anticipationPointsPool.Remove(nextPoint);

            nextPoint.ShowAnticipationImage();
        }
    }
}
