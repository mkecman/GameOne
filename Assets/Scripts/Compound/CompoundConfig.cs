using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompoundConfig : Dictionary<int, CompoundJSON>
{
    internal void Setup()
    {
        foreach( KeyValuePair<int, CompoundJSON> item in this )
        {
            item.Value.Texture = Resources.Load( "CompoundTexture/" + item.Value.Index ) as Texture2D;
        }
    }
}
