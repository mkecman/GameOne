using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor( typeof( CompoundEditor ) )]
public class CompoundEditorEditor : Editor
{
    private CompoundEditor _compoundEditor;

    public override void OnInspectorGUI()
    {
        _compoundEditor = (CompoundEditor)target;

        EditorGUILayout.PropertyField( serializedObject.FindProperty( "Index" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "Effect" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "Level" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "IsPositive" ) );
        EditorGUILayout.PropertyField( serializedObject.FindProperty( "CompoundType" ) );
        
        EditorGUILayout.BeginHorizontal();

        if( GUILayout.Button( "Generate" ) )
        {
            _compoundEditor.GenerateCompound();
        }

        if( GUILayout.Button( "Export" ) )
        {
            Debug.Log( JsonConvert.SerializeObject( _compoundEditor.Compound ) );
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField( serializedObject.FindProperty( "Compound" ), true );

        EditorGUILayout.PropertyField( serializedObject.FindProperty( "CompoundGenerator" ) );

        serializedObject.ApplyModifiedProperties();
    }

    void OnEnable()
    {
        
    }

}
