using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttonify : MonoBehaviour
{
    BuildingObject parentObject;

    private void OnMouseDown()
    {
        parentObject = transform.parent.GetComponent<BuildingObject>();

        //parentObject.BuildingSelected();
    }
}
