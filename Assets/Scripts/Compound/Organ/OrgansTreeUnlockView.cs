using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using UnityEditor;
using System.IO;

public class OrgansTreeUnlockView : GameView
{
    public TreeBranchData SelectedCompound;
    public GameObject ConnectionPrefab;
    public Transform ConnectionsContainer;
    public GameObject ItemPrefab;
    public Transform ItemsContainer;
    public CompoundTreeConfig TreeConfig;

    public void Draw()
    {
        RemoveAllChildren( ItemsContainer );
        RemoveAllChildren( ConnectionsContainer );
        if( TreeConfig != null )
            DrawChildrenRecursive( TreeConfig, null );
        else
            Debug.Log( "Please press Load first to load data from the config!" );
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
        if( TreeConfig != null )
        {
            File.WriteAllText(
            "Assets/Resources/Configs/CompoundTreeConfig.json",
            JsonConvert.SerializeObject( TreeConfig )
            );
            Debug.Log( "CompoundTreeConfig Saved" );
            AssetDatabase.Refresh();
        }
        else
            Debug.Log( "Please press Load first to load data from the config!" );
    }

    public void Load()
    {
        TreeConfig = JsonConvert.DeserializeObject<CompoundTreeConfig>
                ( Resources.Load<TextAsset>( "Configs/CompoundTreeConfig" ).text );
        Debug.Log( "Loaded" );
    }

    private void DrawChildrenRecursive( TreeBranchData branch, GameObject parent )
    {
        GameObject parentGO = Add( branch );
        AddConnection( branch, parentGO, parent );
        foreach( TreeBranchData child in branch.Children )
        {
            DrawChildrenRecursive( child, parentGO );
        }
    }

    private GameObject Add( TreeBranchData branchData )
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

}
