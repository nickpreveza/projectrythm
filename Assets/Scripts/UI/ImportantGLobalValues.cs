using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantGLobalValues : MonoBehaviour
{
    private static ImportantGLobalValues _instance;

    public static ImportantGLobalValues Instance { get { return _instance; } }

   

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
