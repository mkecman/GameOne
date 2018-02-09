using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer( typeof( FractalModel ) )]
public class FractalModelPropertyDrawer : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        EditorGUI.PropertyField( position, property, label, true );
    }

    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return EditorGUI.GetPropertyHeight( property );
    }
}
