using PsiPhi;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlanetProperty
{
    public float Variation;
    internal WeightedValue[] HexDistribution = new WeightedValue[101];

    [SerializeField]
    internal DoubleReactiveProperty _Value = new DoubleReactiveProperty();
    public double Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

    public PlanetProperty( double value, float variation )
    {
        Value = value;
        Variation = variation;

        for( int i = 0; i <= 100; i++ )
            HexDistribution[ i ] = new WeightedValue( PPMath.Round( i / 100f ), 0 );
    }
}
