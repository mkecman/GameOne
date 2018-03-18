using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class BellCurve
{
    public FloatReactiveProperty Amplitude = new FloatReactiveProperty( 1 );
    public FloatReactiveProperty Position = new FloatReactiveProperty( 0 );
    public FloatReactiveProperty Range = new FloatReactiveProperty( 0.1f );

    public IntReactiveProperty Level = new IntReactiveProperty(0);

    private float _defaultPosition;

    public BellCurve()
    {

    }

    public BellCurve( float amplitude = 1, float position = 0, float range = 0.1f )
    {
        Amplitude.Value = amplitude;
        Position.Value = position;
        _defaultPosition = position;
        Range.Value = range;
    }

    public float GetValueAt( double time )
    {
        return (float)( Amplitude.Value * Math.Exp( -Math.Pow( time - Position.Value, 2 ) / ( 2 * Math.Pow( Range.Value, 2 ) ) ) );
    }

    public bool ChangePosition( float delta )
    {
        Position.Value += delta;
        bool increase = false;

        if( delta > 0 )
        {
            if( Position.Value > _defaultPosition )
            {
                Level.Value++;
                increase = true;
            }
            if( Position.Value < _defaultPosition )
                Level.Value--;
        }

        if( delta < 0 )
        {
            if( Position.Value > _defaultPosition )
                Level.Value--;
            if( Position.Value < _defaultPosition )
            {
                Level.Value++;
                increase = true;
            }
        }
        return increase;
    }
    
}
