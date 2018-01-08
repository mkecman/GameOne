using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor( typeof( ObjectComponent ) )]
public class EObjectEditor : Editor
{
    private ReorderableList list;
    
    public override void OnInspectorGUI()
    {
        ObjectComponent oc = (ObjectComponent)target;

        serializedObject.Update();


        EditorList.Show( serializedObject.FindProperty( "_InputConnections" ), EditorListOption.Buttons | EditorListOption.ListLabel );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Value" ) );
        //DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();

    }
}

[CustomEditor( typeof( EConnections ) )]
public class EConnectionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EConnections oc = (EConnections)target;
        serializedObject.Update();
        if( GUILayout.Button( "Refresh" ) )
        {
            oc.Refresh();
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
