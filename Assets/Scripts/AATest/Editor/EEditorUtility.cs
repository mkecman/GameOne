using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[Flags]
public enum EEditorUtilityOptions
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}

public static class EEditorUtility
{

    private static GUIContent
        moveUpButtonContent = new GUIContent( "\u2191", "move up" ),
        moveDownButtonContent = new GUIContent( "\u2193", "move down" ),
        duplicateButtonContent = new GUIContent( "+", "duplicate" ),
        deleteButtonContent = new GUIContent( "-", "delete" ),
        addButtonContent = new GUIContent( "+", "add element" );

    private static GUILayoutOption miniButtonWidth = GUILayout.Width( 20f );

    public static void Show( SerializedProperty list, EEditorUtilityOptions options = EEditorUtilityOptions.Default )
    {
        if( !list.isArray )
        {
            EditorGUILayout.HelpBox( list.name + " is neither an array nor a list!", MessageType.Error );
            return;
        }

        bool
            showListLabel = ( options & EEditorUtilityOptions.ListLabel ) != 0,
            showListSize = ( options & EEditorUtilityOptions.ListSize ) != 0;

        if( showListLabel )
        {
            EditorGUILayout.PropertyField( list );
            EditorGUI.indentLevel += 1;
        }
        if( !showListLabel || list.isExpanded )
        {

            SerializedProperty size = list.FindPropertyRelative( "Array.size" );
            if( showListSize )
            {
                EditorGUILayout.PropertyField( size );
            }
            if( size.hasMultipleDifferentValues )
            {
                EditorGUILayout.HelpBox( "Not showing lists with different sizes.", MessageType.Info );
            }
            else
            {
                ShowElements( list, options );
            }
        }
        if( showListLabel )
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static void ShowElements( SerializedProperty list, EEditorUtilityOptions options )
    {
        bool
            showElementLabels = ( options & EEditorUtilityOptions.ElementLabels ) != 0,
            showButtons = ( options & EEditorUtilityOptions.Buttons ) != 0;

        for( int i = 0; i < list.arraySize; i++ )
        {
            if( showButtons )
            {
                EditorGUILayout.BeginHorizontal();
            }
            if( showElementLabels )
            {
                EditorGUILayout.PropertyField( list.GetArrayElementAtIndex( i ) );
            }
            else
            {
                EditorGUILayout.PropertyField( list.GetArrayElementAtIndex( i ), GUIContent.none );
            }
            if( showButtons )
            {
                ShowButtons( list, i );
                EditorGUILayout.EndHorizontal();
            }
        }
        if( showButtons && list.arraySize == 0 && GUILayout.Button( addButtonContent, EditorStyles.miniButton ) )
        {
            list.arraySize += 1;
            list.serializedObject.ApplyModifiedProperties();
            ENode objectComponent = list.serializedObject.targetObject as ENode;
            objectComponent.AddConnection( 0 );
        }
    }

    private static void ShowButtons( SerializedProperty list, int index )
    {
        ENode objectComponent = list.serializedObject.targetObject as ENode;

        if( GUILayout.Button( moveUpButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth ) )
        {
            int size = list.arraySize;
            list.InsertArrayElementAtIndex( size );
            list.MoveArrayElement( index - 1, size );
            list.MoveArrayElement( index, index - 1 );
            list.MoveArrayElement( size, index );
            list.DeleteArrayElementAtIndex( size );

            list.serializedObject.ApplyModifiedProperties();
        }
        if( GUILayout.Button( moveDownButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth ) )
        {
            int size = list.arraySize;
            list.InsertArrayElementAtIndex( size );
            list.MoveArrayElement( index + 1, size );
            list.MoveArrayElement( index, index + 1 );
            list.MoveArrayElement( size, index );
            list.DeleteArrayElementAtIndex( size );

            list.serializedObject.ApplyModifiedProperties();
            //objectComponent.AddConnection( index + 1 );
        }
        if( GUILayout.Button( duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth ) )
        {
            list.InsertArrayElementAtIndex( index );
            list.serializedObject.ApplyModifiedProperties();
            objectComponent.AddConnection( index + 1 );
        }
        if( GUILayout.Button( deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth ) )
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex( index );
            if( list.arraySize == oldSize )
            {
                list.DeleteArrayElementAtIndex( index );
            }
            list.serializedObject.ApplyModifiedProperties();
            objectComponent.RemoveConnection();
        }
    }

    public static object GetParent( SerializedProperty prop )
    {
        var path = prop.propertyPath.Replace( ".Array.data[", "[" );
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split( '.' );
        foreach( var element in elements.Take( elements.Length ) )
        {
            if( element.Contains( "[" ) )
            {
                var elementName = element.Substring( 0, element.IndexOf( "[" ) );
                var index = Convert.ToInt32( element.Substring( element.IndexOf( "[" ) ).Replace( "[", "" ).Replace( "]", "" ) );
                obj = GetValue( obj, elementName, index );
            }
            else
            {
                obj = GetValue( obj, element );
            }
        }
        return obj;
    }

    public static object GetValue( object source, string name )
    {
        if( source == null )
            return null;
        var type = source.GetType();
        var f = type.GetField( name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
        if( f == null )
        {
            var p = type.GetProperty( name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
            if( p == null )
                return null;
            return p.GetValue( source, null );
        }
        return f.GetValue( source );
    }

    public static object GetValue( object source, string name, int index )
    {
        var enumerable = GetValue( source, name ) as IEnumerable;
        var enm = enumerable.GetEnumerator();
        while( index-- >= 0 )
            enm.MoveNext();
        return enm.Current;
    }
}