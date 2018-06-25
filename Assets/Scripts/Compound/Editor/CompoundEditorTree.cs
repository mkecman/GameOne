using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CompoundEditorTree : TreeView
{
    public CompoundTreeConfig RootConfig;
    public int TotalItems;
    public OrgansTreeUnlockView ComponentInstance;
    private int _currentDragIndex;

    public CompoundEditorTree( TreeViewState treeViewState ) : base( treeViewState )
    {
        showBorder = true;
    }

    protected override TreeViewItem BuildRoot()
    {
        // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
        // are created from data. Here we just create a fixed set of items, in a real world example
        // a data model should be passed into the TreeView and the items created from the model.
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
        root.children = new List<TreeViewItem>();
        TotalItems = 1;

        if( RootConfig == null )
            return root;


        foreach( TreeBranchData branch in RootConfig.Children )
        {
            TreeViewItem treeViewItem = new TreeViewItem( branch.Index, 0, branch.Name );
            treeViewItem.parent = root;
            treeViewItem.children = GetAllItems( branch, treeViewItem );

            root.AddChild( treeViewItem );
            TotalItems++;
        }
        
        // Utility method that initializes the TreeViewItem.children and -parent for all items.
        //SetupParentsAndChildrenFromDepths( root, allItems );
        SetupDepthsFromParentsAndChildren( root );

        // Return root of the tree
        return root;
    }

    private List<TreeViewItem> GetAllItems( TreeBranchData branch, TreeViewItem parentItem )
    {
        List<TreeViewItem> items = new List<TreeViewItem>();
        foreach( TreeBranchData child in branch.Children )
        {
            TreeViewItem treeViewItem = new TreeViewItem( child.Index, 0, child.Name );
            treeViewItem.parent = parentItem;
            treeViewItem.children = GetAllItems( child, treeViewItem );

            items.Add( treeViewItem );
            TotalItems++;
        }

        return items;
    }

    protected override void SetupDragAndDrop( SetupDragAndDropArgs args )
    {
        DragAndDrop.PrepareStartDrag();
        _currentDragIndex = args.draggedItemIDs[ 0 ];
        DragAndDrop.objectReferences = new UnityEngine.Object[1]; //needed for drag operation to accept drops
        DragAndDrop.StartDrag( "dragging" );
    }

    protected override DragAndDropVisualMode HandleDragAndDrop( DragAndDropArgs args )
    {
        // Reparent
        if( args.performDrop )
        {
            switch( args.dragAndDropPosition )
            {
                case DragAndDropPosition.UponItem:
                case DragAndDropPosition.BetweenItems:
                    TreeBranchData branch = ComponentInstance.GetBranch( _currentDragIndex, RootConfig.Children );

                    TreeBranchData branchParent = RootConfig;
                    if( branch.ParentIndex > 0 )
                        branchParent = ComponentInstance.GetBranch( branch.ParentIndex, RootConfig.Children );

                    TreeBranchData dropParent = RootConfig;
                    if( args.parentItem.id > 0 ) //Root object is id=0
                        dropParent = ComponentInstance.GetBranch( args.parentItem.id, RootConfig.Children );
                    int insertAtIndex;
                    if( branchParent == dropParent ) //if moving within same parent, we move item in the list
                    {
                        int oldIndex = branchParent.Children.IndexOf( branch );
                        branchParent.Children.RemoveAt( oldIndex );
                        insertAtIndex = args.insertAtIndex > oldIndex ? args.insertAtIndex-1 : args.insertAtIndex;
                        dropParent.Children.Insert( insertAtIndex, branch );
                    }
                    else //we add new item to new parent and remove the old branch
                    {
                        insertAtIndex = args.insertAtIndex == -1 ? dropParent.Children.Count : args.insertAtIndex;
                        dropParent.Children.Insert( insertAtIndex, branch );
                        branch.ParentIndex = dropParent.Index;

                        branchParent.Children.Remove( branch );
                    }
                    Reload();
                    break;

                case DragAndDropPosition.OutsideItems:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            SetSelection( new List<int> { _currentDragIndex }, TreeViewSelectionOptions.RevealAndFrame );
        }
        
        return DragAndDropVisualMode.Move;
    }

    protected override void SelectionChanged( IList<int> selectedIds )
    {
        ComponentInstance.SelectedCompound = ComponentInstance.GetBranch( selectedIds[ 0 ], RootConfig.Children );
        ComponentInstance.CompoundEditor.Compound = ComponentInstance.CompoundListConfig[ selectedIds[ 0 ] ];
    }

    protected override bool CanStartDrag( CanStartDragArgs args )
    {
        return true;
    }

    
}
