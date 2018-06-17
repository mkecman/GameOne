using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreeBranchData
{
    public int Index;
    public int ParentIndex;
    public string Name;
    public bool IsUnlocked;
    [HideInInspector]
    public List<TreeBranchData> Children = new List<TreeBranchData>();

    public TreeBranchData()
    {
    }

    public TreeBranchData( int index, string name = "New Branch" )
    {
        Index = index;
        Name = name;
    }
}
