using UnityEngine;
using System.Collections;
using UniRx;

public class BellCurve
{
    public FloatReactiveProperty Amplitude = new FloatReactiveProperty( 1 );
    public FloatReactiveProperty Position = new FloatReactiveProperty( 0 );
    public FloatReactiveProperty Range = new FloatReactiveProperty( 0.1f );

    public BellCurve( float amplitude, float position, float range )
    {
        Amplitude.Value = amplitude;
        Position.Value = position;
        Range.Value = range;
    }

    public float GetValueAt( float time )
    {
        return Amplitude.Value * Mathf.Exp( -Mathf.Pow( time - Position.Value, 2 ) / ( 2 * Mathf.Pow( Range.Value, 2 ) ) );
    }
}
