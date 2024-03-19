using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalleteHandler : MonoBehaviour
{
    public Color activeButton;
    public Color highlightedButton;
    public Color clickedButton;
    public Color disabledButton;

    public Color foregroundColor1;
    public Color foregroundColor2;
    public Color backgroundColor1;
    public Color backgroundColor2;


    Color ColorFromGradient(Gradient target, float value)  // float between 0-1
    {
        return target.Evaluate(value);
    }

}
