using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer( typeof( RDictionary ) )]
public class RDictionaryDrawer : PropertyDrawer
{
    private CompoundEditor compoundEditor;

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        EditorGUILayout.LabelField( "Effects" );

        compoundEditor = property.serializedObject.targetObject as CompoundEditor;
        compoundEditor.MakeLists();
        SerializedProperty _keys = property.serializedObject.FindProperty( "_Keys" );
        SerializedProperty _values = property.serializedObject.FindProperty( "_Values" );
        
        for( int i = 0; i < _keys.arraySize; i++ )
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField( _keys.GetArrayElementAtIndex( i ), new GUIContent( "" ) );
            EditorGUILayout.PropertyField( _values.GetArrayElementAtIndex( i ), new GUIContent( "" ) );

            if( EditorGUI.EndChangeCheck() )
            {
                property.serializedObject.ApplyModifiedProperties();
                compoundEditor.UpdateLists();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    /**/
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return 1f;
        //return label != GUIContent.none && Screen.width < 333 ? ( 16f + 18f ) : 16f;
    }
    /**/
}
