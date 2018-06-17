using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor( typeof( CompoundEditor ) )]
public class CompoundEditorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompoundEditor editor = (CompoundEditor)target;

        Rect r = EditorGUILayout.BeginHorizontal( "Window", GUILayout.ExpandHeight( true ) );
        m_SimpleTreeView.OnGUI( r );
        EditorGUILayout.EndHorizontal();


        if( GUILayout.Button( "Add Compound" ) )
        {
            _compoundTreeConfig.Children.Add( new TreeBranchData( m_SimpleTreeView.TotalItems, m_SimpleTreeView.TotalItems.ToString() ) );
            m_SimpleTreeView.Reload();
        }


        if( GUILayout.Button( "Load" ) )
        {
            _compoundTreeConfig = GameConfig.Get<CompoundTreeConfig>();
            m_SimpleTreeView.Editor = editor;
            m_SimpleTreeView.RootConfig = _compoundTreeConfig;
            m_SimpleTreeView.Reload();
        }

        if( GUILayout.Button( "Save" ) )
        {
            File.WriteAllText(
            "Assets/Resources/Configs/CompoundTreeConfig.json",
            JsonConvert.SerializeObject( _compoundTreeConfig )
            );
            Debug.Log( "CompoundTreeConfig Saved" );
        }




        DrawDefaultInspector();
    }

    [SerializeField] TreeViewState m_TreeViewState;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    CompoundEditorTree m_SimpleTreeView;
    private CompoundTreeConfig _compoundTreeConfig;

    void OnEnable()
    {

        // Check whether there is already a serialized view state (state 
        // that survived assembly reloading)
        if( m_TreeViewState == null )
            m_TreeViewState = new TreeViewState();

        m_SimpleTreeView = new CompoundEditorTree( m_TreeViewState );
        m_SimpleTreeView.Reload();
    }

}
