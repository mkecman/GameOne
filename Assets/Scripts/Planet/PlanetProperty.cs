using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlanetProperty
{
    public float Value;
    public float Variation;
    internal List<WeightedValue> HexDistribution;

    [SerializeField]
    internal FloatReactiveProperty _AvgValue = new FloatReactiveProperty();
    public float AvgValue
    {
        get { return _AvgValue.Value; }
        set { _AvgValue.Value = value; }
    }


    public PlanetProperty( float value, float variation )
    {
        Value = value;
        Variation = variation;
    }
}
