using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public class OrgansTreeUnlockView : GameView
{
    public TreeBranchData SelectedCompound;
    public GameObject ConnectionPrefab;
    public Transform ConnectionsContainer;
    public GameObject ItemPrefab;
    public Transform ItemsContainer;
    public CompoundEditor CompoundEditor;

    public CompoundTreeConfig CompoundTreeConfig;
    public CompoundConfig CompoundListConfig;

    void Start()
    {
        Load();
        Draw();
    }

    public void AddGeneratedCompound()
    {
        CompoundTreeConfig.Children.Add( new TreeBranchData( CompoundEditor.Compound.Index, CompoundEditor.Compound.Name ) );
        CompoundListConfig.Add( CompoundEditor.Compound.Index, GameModel.Copy( CompoundEditor.Compound ) );
    }

    public void RemoveSelectedItem()
    {
        TreeBranchData parent = GetBranch( SelectedCompound.ParentIndex, CompoundTreeConfig.Children );
        parent.Children.Remove( SelectedCompound );
        CompoundListConfig.Remove( SelectedCompound.Index );
    }

    public void Draw()
    {
        RemoveAllChildren( ItemsContainer );
        RemoveAllChildren( ConnectionsContainer );
        DrawChildrenRecursive( CompoundTreeConfig, null );
    }

    public void UpdateConnections()
    {
        for( int i = 0; i < ConnectionsContainer.childCount; i++ )
        {
            ConnectionsContainer.GetChild( i ).GetComponent<OrgansTreeUnlockViewConnection>().Update();
        }

        Transform go;
        OrgansTreeUnlockViewItem item;
        for( int i = 0; i < ItemsContainer.childCount; i++ )
        {
            go = ItemsContainer.GetChild( i );
            item = go.GetComponent<OrgansTreeUnlockViewItem>();
            item.BranchData.X = go.position.x;
            item.BranchData.Y = go.position.y;
        }
    }

    public void Save()
    {
        File.WriteAllText(
        "Assets/Resources/Configs/CompoundTreeConfig.json",
        JsonConvert.SerializeObject( CompoundTreeConfig )
        );
        File.WriteAllText(
        "Assets/Resources/Configs/CompoundConfig.json",
        JsonConvert.SerializeObject( CompoundListConfig )
        );
        Debug.Log( "Compound Configs Saved" );
        AssetDatabase.Refresh();
    }

    public void Load( bool forceReload = false )
    {
        if( CompoundTreeConfig == null || forceReload )
        {
            CompoundTreeConfig = JsonConvert.DeserializeObject<CompoundTreeConfig>
                    ( Resources.Load<TextAsset>( "Configs/CompoundTreeConfig" ).text );
            CompoundListConfig = JsonConvert.DeserializeObject<CompoundConfig>
                    ( Resources.Load<TextAsset>( "Configs/CompoundConfig" ).text );

            UpdateCompoundNames( CompoundTreeConfig );

            Debug.Log( "Compound Configs Loaded" );
        }
    }

    private void UpdateCompoundNames( TreeBranchData branch )
    {
        branch.Name = CompoundListConfig[ branch.Index ].Name;
        foreach( TreeBranchData child in branch.Children )
        {
            UpdateCompoundNames( child );
        }
    }

    private void DrawChildrenRecursive( TreeBranchData branch, GameObject parent )
    {
        GameObject parentGO = AddItem( branch );
        AddConnection( branch, parentGO, parent );
        foreach( TreeBranchData child in branch.Children )
        {
            DrawChildrenRecursive( child, parentGO );
        }
    }

    private GameObject AddItem( TreeBranchData branchData )
    {
        GameObject go = Instantiate( ItemPrefab, ItemsContainer );
        go.GetComponent<OrgansTreeUnlockViewItem>().Setup( branchData );
        return go;
    }

    private void AddConnection( TreeBranchData branch, GameObject source, GameObject parent )
    {
        GameObject go = Instantiate( ConnectionPrefab, ConnectionsContainer );
        go.GetComponent<OrgansTreeUnlockViewConnection>().Setup( branch, source, parent );
    }

    public TreeBranchData GetBranch( int index, List<TreeBranchData> children )
    {
        TreeBranchData branch = null;
        foreach( TreeBranchData child in children )
        {
            if( child.Index == index )
                return child;
            if( child.Children.Count > 0 )
            {
                branch = GetBranch( index, child.Children );
                if( branch != null )
                    return branch;
            }
        }

        return branch;
    }

}
