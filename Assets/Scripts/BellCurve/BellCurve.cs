using System;
using UniRx;
using PsiPhi;
using UnityEngine;

public class BellCurve
{
    public FloatReactiveProperty Amplitude = new FloatReactiveProperty( 1 );
    public FloatReactiveProperty Range = new FloatReactiveProperty( .1f );
    public FloatReactiveProperty Position = new FloatReactiveProperty( .5f );
    public IntReactiveProperty Consumption = new IntReactiveProperty( 0 );

    private float _defaultPosition = float.MaxValue;

    public BellCurve(){}

    public BellCurve( float amplitude = 1, float position = .5f, float range = .1f )
    {
        Amplitude.Value = amplitude;
        Position.Value = position;
        Range.Value = range;
    }

    public float GetFloatAt( float time )
    {
        return Amplitude.Value * Mathf.Exp( -Mathf.Pow( time - Position.Value, 2 ) / ( 2 * Mathf.Pow( Range.Value, 2 ) ) );
    }

    public int GetIntAt( float time )
    {
        return (int)( Amplitude.Value * Mathf.Exp( -Mathf.Pow( time - Position.Value, 2 ) / ( 2 * Mathf.Pow( Range.Value, 2 ) ) ) * 100 );
    }

    public bool ChangePosition( float delta )
    {
        //because JSON parser sets Position outside of the constructor, we have to take the latest value here instead
        if( _defaultPosition == float.MaxValue )
            _defaultPosition = Position.Value;

        Position.Value = PPMath.Round( Position.Value + delta, 2 );
        bool increase = false;


        if( delta > 0 )
        {
            if( Position.Value > _defaultPosition )
            {
                Consumption.Value++;
                increase = true;
            }
            if( Position.Value <= _defaultPosition )
                Consumption.Value--;
        }

        if( delta < 0 )
        {
            if( Position.Value >= _defaultPosition )
                Consumption.Value--;
            if( Position.Value < _defaultPosition )
            {
                Consumption.Value++;
                increase = true;
            }
        }

        return increase;
    }

}
