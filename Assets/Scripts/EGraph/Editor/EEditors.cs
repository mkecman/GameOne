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
        GUIContent addButtonContent = new GUIContent( "PROCESS" );
        if( GUILayout.Button( addButtonContent, EditorStyles.miniButton ) )
        {
            oc.Process();
        }
         
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_AutoTrigger" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Name" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Value" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_Delta" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_MinValue" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "_MaxValue" ) );
        EEditorUtility.Show( serializedObject.FindProperty( "_SourceConnections" ), EEditorUtilityOptions.Buttons | EEditorUtilityOptions.ListLabel );
        EEditorUtility.Show( serializedObject.FindProperty( "_TargetConnections" ), EEditorUtilityOptions.Buttons | EEditorUtilityOptions.ListLabel );
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
