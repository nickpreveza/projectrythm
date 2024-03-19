using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsManager : MonoBehaviour
{
    public float buttonRotationSpeed;
    public Sprite earlyState;
    public Sprite perfectState;
    public Sprite lateState;

    public Sprite earlyState_interacted;
    public Sprite perfectState_interacted;
    public Sprite lateState_interacted;

    public Sprite[] topBlocks;
    public Sprite[] midBlocks;
    public Sprite[] bottomBlocks;
    public Sprite[] narrowTopBlocks;
    public Sprite[] narrowMidBlocks;
    public Sprite[] narrowBottomBlocks;
    public GameObject[] explosions;

    [SerializeField] Sprite[] feedbackSprites; //0 miss, 1 early, 2 perfect, 3 late

    public Color[] meteoriteColors;
    public Color[] buildingColors;
    public Color[] narrowBuildingColors;
}
