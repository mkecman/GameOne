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


        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Name" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Value" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Delta" ) );
        EEditorUtility.Show( serializedObject.FindProperty( "_TargetConnections" ), EEditorUtilityOptions.Buttons | EEditorUtilityOptions.ListLabel );
        EEditorUtility.Show( serializedObject.FindProperty( "_SourceConnections" ), EEditorUtilityOptions.ListLabel );
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
        if( GUILayout.Button( "Redraw" ) )
        {
            oc.Redraw();
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
