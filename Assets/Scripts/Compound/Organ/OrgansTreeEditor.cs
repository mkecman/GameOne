using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OrgansTreeEditor : GameView
{
    public TreeBranchData SelectedCompound;

    public OrgansTreeView TreeView;
    public CompoundEditor CompoundEditor;

    public CompoundTreeConfig CompoundTreeConfig;
    public CompoundConfig CompoundListConfig;
    private float _currentHeight;

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
        TreeBranchData parent = CompoundTreeConfig.GetBranch( SelectedCompound.ParentIndex );
        RemoveRecursive( SelectedCompound );
        parent.Children.Remove( SelectedCompound );
    }

    private void RemoveRecursive( TreeBranchData branch )
    {
        foreach( TreeBranchData child in branch.Children )
            RemoveRecursive( child );

        CompoundListConfig.Remove( branch.Index );
    }

    public void Draw()
    {
        TreeView.OnBranchChange( CompoundTreeConfig );
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
        if( Application.isPlaying && ( CompoundTreeConfig == null || forceReload ) )
        {
            CompoundTreeConfig = GameConfig.Get<CompoundTreeConfig>();
            CompoundListConfig = GameConfig.Get<CompoundConfig>();
            Debug.Log( "Application.isPlaying = true" );
        }
        if( !Application.isPlaying && ( CompoundTreeConfig == null || forceReload ) )
        {
            CompoundTreeConfig = JsonConvert.DeserializeObject<CompoundTreeConfig>
                    ( Resources.Load<TextAsset>( "Configs/CompoundTreeConfig" ).text );
            CompoundListConfig = JsonConvert.DeserializeObject<CompoundConfig>
                    ( Resources.Load<TextAsset>( "Configs/CompoundConfig" ).text );
            Debug.Log( "Application.isPlaying = false" );
        }
        UpdateCompoundNames( CompoundTreeConfig );
        Debug.Log( "Compound Configs Loaded" );
    }

    private void UpdateCompoundNames( TreeBranchData branch )
    {
        branch.Name = CompoundListConfig[ branch.Index ].Name;
        foreach( TreeBranchData child in branch.Children )
        {
            UpdateCompoundNames( child );
        }
    }
}
