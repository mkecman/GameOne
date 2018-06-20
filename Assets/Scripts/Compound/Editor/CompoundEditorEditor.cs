﻿using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor( typeof( CompoundEditor ) )]
public class CompoundEditorEditor : Editor
{
    [SerializeField]
    private TreeViewState m_TreeViewState;
    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    private CompoundEditorTree m_SimpleTreeView;
    private CompoundTreeConfig _treeConfig;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginScrollView( new Vector2(), GUILayout.MaxHeight(800f), GUILayout.ExpandHeight(true) );
        m_SimpleTreeView.OnGUI( EditorGUILayout.GetControlRect( GUILayout.ExpandHeight( true ) ) );
        EditorGUILayout.EndScrollView();

        if( GUILayout.Button( "Add Item" ) )
        {
            if( _treeConfig != null )
            {
                _treeConfig.Children.Add( new TreeBranchData( m_SimpleTreeView.TotalItems, m_SimpleTreeView.TotalItems.ToString() ) );
                m_SimpleTreeView.Reload();
            }
            else
                Debug.Log( "Please press Load first to load data from the config!" );
        }

        DrawDefaultInspector();
    }

    void OnEnable()
    {
        if( m_TreeViewState == null )
            m_TreeViewState = new TreeViewState();

        m_SimpleTreeView = new CompoundEditorTree( m_TreeViewState );
        m_SimpleTreeView.Reload();
    }

}
