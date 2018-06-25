using System;
using System.Collections.Generic;
using UnityEngine;

public class CompoundEditor : MonoBehaviour
{
    public int Index;
    public R Effect;
    public int Level;
    public bool IsPositive;
    public CompoundType CompoundType;
    public CompoundJSON Compound;
    public CompoundGenerator CompoundGenerator;

    public void GenerateCompound()
    {
        Compound = CompoundGenerator.CreateCompound( Index, Effect, Level, IsPositive, this.CompoundType );
    }

    public List<R> _Keys;
    public List<float> _Values;

    public void MakeLists()
    {
        _Keys.Clear();
        _Values.Clear();
        foreach( var kvp in Compound.Effects )
        {
            _Keys.Add( kvp.Key );
            _Values.Add( kvp.Value );
        }
    }

    public void UpdateLists()
    {
        Compound.Effects.Clear();
        for( int i = 0; i < _Keys.Count; i++ )
        {
            Compound.Effects.Add( _Keys[ i ], _Values[ i ] );
        }
    }
}
