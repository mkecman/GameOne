using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor( typeof( ENode ) )]
public class ENodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ENode oc = (ENode)target;

        serializedObject.Update();


        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Name" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Value" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Delta" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_MinValue" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_MaxValue" ) );
        EEditorUtility.Show( serializedObject.FindProperty( "_TargetConnections" ), EEditorUtilityOptions.Buttons | EEditorUtilityOptions.ListLabel );
        EEditorUtility.Show( serializedObject.FindProperty( "_SourceConnections" ), EEditorUtilityOptions.ListLabel );
        //DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();

    }
}

[CustomEditor( typeof( EEdges ) )]
public class EEdgesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EEdges oc = (EEdges)target;
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
