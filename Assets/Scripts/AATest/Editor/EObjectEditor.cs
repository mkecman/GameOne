using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor( typeof( EObject ) )]
public class EObjectEditor : Editor
{
    private ReorderableList list;
    
    public override void OnInspectorGUI()
    {
        EObject oc = (EObject)target;

        serializedObject.Update();


        //EEditorUtility.Show( serializedObject.FindProperty( "_InputConnections" ), EEditorUtilityOptions.Buttons | EEditorUtilityOptions.ListLabel );
        //EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Value" ) );
        DrawDefaultInspector();

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
        if( GUILayout.Button( "Redraw" ) )
        {
            oc.Redraw();
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
