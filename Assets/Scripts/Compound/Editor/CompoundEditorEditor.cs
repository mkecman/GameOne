using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof( CompoundEditor ) )]
public class CompoundEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompoundEditor editor = (CompoundEditor)target;
        if( GUILayout.Button( "Load" ) )
            editor.Load();

        if( GUILayout.Button( "Save" ) )
            editor.Save();

        DrawDefaultInspector();
    }
}
