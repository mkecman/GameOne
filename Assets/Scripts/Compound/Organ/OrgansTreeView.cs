using System;
using UnityEngine;

public class OrgansTreeView : GameView
{
    public float ItemHeight = 100f;
    public float XPositionStep = 100f;
    public int MaxDepth = 10;
    public float Padding = 100f;
    public GameObject ConnectionPrefab;
    public Transform ConnectionsContainer;
    public GameObject ItemPrefab;
    public Transform ItemsContainer;
    public RectTransform OrgansPanel;

    private TreeBranchData Branch;
    private float _currentHeight;
    private float _maxWidth;

    void Start()
    {
        GameModel.HandleGet<TreeBranchData>( OnBranchChange );
    }

    public void OnBranchChange( TreeBranchData branch )
    {
        Branch = branch;
        Draw();
    }

    private void Draw()
    {
        RemoveAllChildren( ItemsContainer );
        RemoveAllChildren( ConnectionsContainer );
        _currentHeight = -Padding;
        _maxWidth = Padding;

        DrawChildrenRecursive( Branch, null, 0, Padding );

        OrgansPanel.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, _maxWidth );
        OrgansPanel.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, -_currentHeight );
    }

    private void DrawChildrenRecursive( TreeBranchData branch, GameObject parent, int depth = 0, float xPosition = 0f )
    {
        branch.X = xPosition;
        branch.Y = _currentHeight;

        if( _maxWidth < xPosition + Padding )
            _maxWidth = xPosition + Padding;

        GameObject parentGO = AddItem( branch );
        AddConnection( branch, parentGO, parent );

        if( depth < MaxDepth && branch.Children.Count > 0 )
        {
            xPosition += XPositionStep;
            foreach( TreeBranchData child in branch.Children )
            {
                DrawChildrenRecursive( child, parentGO, depth + 1, xPosition );
            }
        }

        if( depth <= 1 )
            _currentHeight -= ItemHeight;
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

    //Save positions if we manually set position in the Unity editor, not used as Draw() is now doing automatic layouting
    private void UpdateConnections()
    {
        for( int i = 0; i < ConnectionsContainer.childCount; i++ )
        {
            ConnectionsContainer.GetChild( i ).GetComponent<OrgansTreeUnlockViewConnection>().Update();
        }

        RectTransform rectTransform;
        OrgansTreeUnlockViewItem item;
        for( int i = 0; i < ItemsContainer.childCount; i++ )
        {
            rectTransform = ItemsContainer.GetChild( i ).GetComponent<RectTransform>();
            item = rectTransform.GetComponent<OrgansTreeUnlockViewItem>();
            item.BranchData.X = rectTransform.anchoredPosition.x;
            item.BranchData.Y = rectTransform.anchoredPosition.y;
        }
    }

}
