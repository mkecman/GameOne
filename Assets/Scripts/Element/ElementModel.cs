using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class ElementModel
{
    public int Index;
    public string Symbol;
    public string Name;
    public double Weight;
    public double Density;
    public string HexColor;
    public string GroupBlock;
    public List<ElementModifierModel> Modifiers;

    public ElementModifierModel Modifier( string Name )
    {
        return Modifiers[ ElementModifiers.IntMap[ Name ] ];
    }
}
