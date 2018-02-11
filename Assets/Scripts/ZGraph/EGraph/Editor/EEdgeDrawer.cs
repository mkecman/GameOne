using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer( typeof( EEdge ) )]
public class EEdgeDrawer : PropertyDrawer
{
    private EEdgesRedrawAll message = new EEdgesRedrawAll();

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        //ENode oc = property.serializedObject.targetObject as ENode;
        EEdge connection = EEditorUtility.GetParent( property ) as EEdge;

        label = EditorGUI.BeginProperty( position, GUIContent.none, property );

        float original = position.width;
        position.width *= 0.3f;
        position.height /= 2;

        EditorGUIUtility.labelWidth = 80.0f;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Source" ), new GUIContent( "Source" ) );
        if( EditorGUI.EndChangeCheck() )
        {
            property.serializedObject.ApplyModifiedProperties();
            connection.SetSource( connection.Source );
            GameMessage.Send<EEdgesRedrawAll>( message );
        }

        position.y += position.height;
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "Target" ), new GUIContent( "Target" ) );
        if( EditorGUI.EndChangeCheck() )
        {
            property.serializedObject.ApplyModifiedProperties();
            connection.SetTarget( connection.Target );
            GameMessage.Send<EEdgesRedrawAll>( message );
        }
        position.y -= position.height;

        position.x = position.width;
        position.width = original - position.width;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "SourceFormula" ), new GUIContent( "Formula" ) );

        position.y += position.height;
        EditorGUI.PropertyField( position, property.FindPropertyRelative( "TargetFormula" ), new GUIContent( "Formula" ) );

        Handles.BeginGUI();
        Handles.color = Color.red;
        Handles.DrawLine( new Vector3( 0, position.y + 19f ), new Vector3( original, position.y + 19f ) );
        Handles.EndGUI();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return 34f;
        //return label != GUIContent.none && Screen.width < 333 ? ( 16f + 18f ) : 16f;
    }
}