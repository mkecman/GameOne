using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( EConnection ) )]
public class EConnectionDrawer : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        EObject oc = property.serializedObject.targetObject as EObject;
        EConnection connection = EEditorUtility.GetParent( property ) as EConnection;

        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty( position, GUIContent.none, property );

        position.width /= 3;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "SourceFormula" ), GUIContent.none );
        position.x += position.width;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "TargetFormula" ), GUIContent.none );
        position.x += position.width;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Target" ), GUIContent.none );
        if( EditorGUI.EndChangeCheck() )
        {
            oc.RemoveConnectionTarget( connection );
            property.serializedObject.ApplyModifiedProperties();
            oc.SetConnectionTarget( connection );
        }
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return 16f;
        //return label != GUIContent.none && Screen.width < 333 ? ( 16f + 18f ) : 16f;
    }
}