﻿using System;
using System.Collections.Generic;

[Serializable]
public class CompoundJSON
{
    public string Name;
    public CompoundType Type;
    public string Formula;
    public float MolecularMass;
    public List<LifeElementModel> Elements;
    //public List<CompoundEffect> Effects;
    public Dictionary<R, float> Effects = new Dictionary<R, float>();
}

public enum CompoundType
{
    Item,
    Armor,
    Weapon
}