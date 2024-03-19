using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    public SimpleBodySourceView sbv;

    public GameObject[] objectToSave;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < objectToSave.Length; i++)
        DontDestroyOnLoad(objectToSave[i]);
    }    
}
