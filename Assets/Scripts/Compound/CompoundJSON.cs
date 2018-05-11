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
    public List<LifeElementModel> Elements = new List<LifeElementModel>();
    public Dictionary<R, float> Effects = new Dictionary<R, float>();

    [SerializeField]
    internal BoolReactiveProperty _CanCraft = new BoolReactiveProperty( true );
    internal bool CanCraft
    {
        get { return _CanCraft.Value; }
        set { _CanCraft.Value = value; }
    }

    internal Texture2D Texture;

}

public enum CompoundType
{
    Item,
    Armor,
    Weapon,
    Consumable
}
