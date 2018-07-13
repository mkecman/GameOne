using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor( typeof( OrgansTreeEditor ) )]
public class OrgansTreeEditorEditor : Editor
{
    [SerializeField]
    private TreeViewState m_TreeViewState;
    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    private CompoundEditorTree m_SimpleTreeView;
    private OrgansTreeEditor _treeView;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginScrollView( new Vector2(), GUILayout.MaxHeight( 800f ), GUILayout.ExpandHeight( true ) );
        m_SimpleTreeView.OnGUI( EditorGUILayout.GetControlRect( GUILayout.ExpandHeight( true ) ) );
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();

        if( GUILayout.Button( "Add" ) )
        {
            _treeView.AddGeneratedCompound();
            m_SimpleTreeView.RootConfig = _treeView.CompoundTreeConfig;
            m_SimpleTreeView.Reload();
        }

        if( GUILayout.Button( "Remove" ) )
        {
            _treeView.RemoveSelectedItem();
            m_SimpleTreeView.RootConfig = _treeView.CompoundTreeConfig;
            m_SimpleTreeView.Reload();
        }

        if( GUILayout.Button( "Load" ) )
        {
            Load( true );
        }

        if( GUILayout.Button( "Draw" ) )
        {
            _treeView.Draw();
        }

        if( GUILayout.Button( "Save" ) )
        {
            _treeView.Save();
        }

        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void Load( bool reload = false )
    {
        _treeView = (OrgansTreeEditor)target;
        _treeView.Load( reload );
        m_SimpleTreeView.ComponentInstance = _treeView;
        m_SimpleTreeView.RootConfig = _treeView.CompoundTreeConfig;
        m_SimpleTreeView.Reload();
    }

    void OnEnable()
    {
        if( m_TreeViewState == null )
            m_TreeViewState = new TreeViewState();

        m_SimpleTreeView = new CompoundEditorTree( m_TreeViewState );

        Load();
    }
}
