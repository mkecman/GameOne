using System;
using System.Collections.Generic;

[Serializable]
public class TreeBranchData
{
    public int Index;
    public int ParentIndex;
    public string Name;
    public bool IsUnlocked;
    public List<TreeBranchData> Children;


    public TreeBranchData()
    {
        Children = new List<TreeBranchData>();
    }

    public TreeBranchData( int index, string name = "New Branch" )
    {
        Index = index;
        Name = name;
        Children = new List<TreeBranchData>();
    }
}
