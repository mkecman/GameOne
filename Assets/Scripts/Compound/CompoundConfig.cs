using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompoundConfig : Dictionary<int, CompoundJSON>
{
    internal void Setup()
    {
        for( int i = 0; i < Count; i++ )
        {
            this[i].Texture = Resources.Load( "CompoundTexture/" + this[i].Index ) as Texture2D;
        }
    }
}
