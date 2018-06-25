using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class CompoundJSON
{
    public int Index;
    public string Name;
    public CompoundType Type;
    public string Formula;
    public float MolecularMass;

    [SerializeField]
    internal BoolReactiveProperty _CanCraft = new BoolReactiveProperty( true );
    internal bool CanCraft
    {
        get { return _CanCraft.Value; }
        set { _CanCraft.Value = value; }
    }

    public List<LifeElementModel> Elements = new List<LifeElementModel>();
    public RDictionary Effects = new RDictionary();

    internal Texture2D Texture;

}

[Serializable]
public class RDictionary : Dictionary<R, float>
{
    //this class is made so we can make property drawer
}

public enum CompoundType
{
    Item,
    Armor,
    Weapon,
    Consumable
}
