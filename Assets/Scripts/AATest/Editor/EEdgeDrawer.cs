using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( EEdge ) )]
public class EEdgeDrawer : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        ENode oc = property.serializedObject.targetObject as ENode;
        EEdge connection = EEditorUtility.GetParent( property ) as EEdge;

        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty( position, GUIContent.none, property );

        EditorGUI.PropertyField( position, property.FindPropertyRelative( "SourceFormula" ), new GUIContent( "Source Formula" ) );
        //position.width /= 2;
        position.height /= 2;
        //position.x += position.width;
        position.y += position.height;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "TargetFormula" ), new GUIContent( "Target Formula" ) );
        position.x += position.width;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Target" ), GUIContent.none );
        if( EditorGUI.EndChangeCheck() )
        {
            oc.RemoveConnectionTarget( connection );
            property.serializedObject.ApplyModifiedProperties();
            oc.SetConnectionTarget( connection );
        }

        Handles.BeginGUI();
        Handles.color = Color.red;
        Handles.DrawLine( new Vector3( 0, position.y + 19f ), new Vector3( position.width * 2, position.y + 19f ) );
        Handles.EndGUI();

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return 34f;
        //return label != GUIContent.none && Screen.width < 333 ? ( 16f + 18f ) : 16f;
    }
}