using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class TreeBranchData
{
    public int Index;
    public int ParentIndex;
    public string Name;
    public float X;
    public float Y;

    [SerializeField]
    internal TreeBranchStateReactiveProperty _State = new TreeBranchStateReactiveProperty();    
    public TreeBranchState State
    {
        get { return _State.Value; }
        set { _State.Value = value; }
    }

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

[Serializable]
public enum TreeBranchState
{
    LOCKED,
    UNLOCKED,
    AVAILABLE,
    ACTIVE
}

[Serializable]
public class TreeBranchStateReactiveProperty : ReactiveProperty<TreeBranchState>
{
    public TreeBranchStateReactiveProperty()
        : base()
    {

    }
}
