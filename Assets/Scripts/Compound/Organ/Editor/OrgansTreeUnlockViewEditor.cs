using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor( typeof( OrgansTreeUnlockView ) )]
public class OrgansTreeUnlockViewEditor : Editor
{
    [SerializeField]
    private TreeViewState m_TreeViewState;
    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    private CompoundEditorTree m_SimpleTreeView;
    private CompoundTreeConfig _treeConfig;

    public override void OnInspectorGUI()
    {
        OrgansTreeUnlockView editor = (OrgansTreeUnlockView)target;
        _treeConfig = editor.TreeConfig;

        EditorGUILayout.BeginScrollView( new Vector2(), GUILayout.MaxHeight( 800f ), GUILayout.ExpandHeight( true ) );
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


        if( GUILayout.Button( "Load" ) )
        {
            editor.Load();
            _treeConfig = editor.TreeConfig;
            m_SimpleTreeView.Editor = editor;
            m_SimpleTreeView.RootConfig = _treeConfig;
            m_SimpleTreeView.Reload();
        }

        if( GUILayout.Button( "Draw" ) )
        {
            editor.Draw();
        }

        if( GUILayout.Button( "Update Connections" ) )
        {
            editor.UpdateConnections();
        }

        if( GUILayout.Button( "Save" ) )
        {
            editor.Save();
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
