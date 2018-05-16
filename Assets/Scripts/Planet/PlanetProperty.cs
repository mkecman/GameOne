using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlanetProperty
{
    public float Variation;
    internal List<WeightedValue> HexDistribution;

    [SerializeField]
    internal DoubleReactiveProperty _Value = new DoubleReactiveProperty();
    public double Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

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
