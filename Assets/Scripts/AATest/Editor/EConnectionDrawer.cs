using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( EConnection ) )]
public class EConnectionDrawer : PropertyDrawer
{

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return label != GUIContent.none && Screen.width < 333 ? ( 16f + 18f ) : 16f;
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty( position, GUIContent.none, property );
        /*Rect contentPosition = EditorGUI.PrefixLabel( position, label );
        if( position.height > 16f )
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect( position );
            contentPosition.y += 18f;
        }
        
        contentPosition.width *= 0.50f;
        EditorGUI.indentLevel = 0;
        */
        position.width /= 4;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Formula" ), GUIContent.none );
        position.x += position.width;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "SourceName" ), GUIContent.none );
        position.x += position.width;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "TargetName" ), GUIContent.none );
        position.x += position.width;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Target" ), GUIContent.none );
        if( EditorGUI.EndChangeCheck() )
        {
            property.serializedObject.ApplyModifiedProperties();
            EObject oc = property.serializedObject.targetObject as EObject;
            EConnection connection = EEditorUtility.GetParent( property ) as EConnection;
            oc.SetConnectionTarget( connection.Index );
        }
        
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }
}