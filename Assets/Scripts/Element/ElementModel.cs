using System;

[Serializable]
public class ElementData
{
    public int Index;
    public string Symbol;
    public string Name;
    public string Group;
    public string Color;
    public float Weight;
    public float Density;
    public float Rarity;
    public ElementRarityClass RarityClass;
}

public enum ElementRarityClass
{
    Abundant,
    Common,
    Uncommon,
    Rare
}
