using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoteGenerator))]
public class NoteGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            NoteGenerator myTarget = (NoteGenerator)target;

            if(GUILayout.Button("Generate Notes"))
            {
                myTarget.GenerateNotes();
            }

            if (GUILayout.Button("Delete Notes"))
            {
                myTarget.DeleteNotes();
            }
        }
    }
}
