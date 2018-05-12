using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlanetProperty
{
    public double Value;
    public float Variation;
    internal List<WeightedValue> HexDistribution;

    [SerializeField]
    internal DoubleReactiveProperty _AvgValue = new DoubleReactiveProperty();
    public double AvgValue
    {
        get { return _AvgValue.Value; }
        set { _AvgValue.Value = value; }
    }

    public PlanetProperty( double value, float variation )
    {
        Value = value;
        Variation = variation;
    }
}
