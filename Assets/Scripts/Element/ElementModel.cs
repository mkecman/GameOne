using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ElementModel
{
    public string Name;
    public string Symbol;
    public int Index;
    public float Weight;
    public string HexColor;
    public string GroupBlock;
    public List<ElementModifierModel> Modifiers;
}
