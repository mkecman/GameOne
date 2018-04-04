using System;
using UniRx;
using PsiPhi;

public class BellCurve
{
    public DoubleReactiveProperty Amplitude = new DoubleReactiveProperty( 1 );
    public IntReactiveProperty Position = new IntReactiveProperty( 0 );
    public DoubleReactiveProperty Range = new DoubleReactiveProperty( 0.1 );

    public IntReactiveProperty Consumption = new IntReactiveProperty( 0 );

    private int _defaultPosition = Int32.MaxValue;

    public BellCurve(){}

    public BellCurve( int amplitude = 100, int position = 100, int range = 100 )
    {
        Amplitude.Value = amplitude / 100f;
        Position.Value = position;
        Range.Value = range / 100f;
    }

    public float GetValueAt( double time )
    {
        return (float)( Amplitude.Value * Math.Exp( -Math.Pow( time - ( Position.Value / 100f ), 2 ) / ( 2 * Math.Pow( Range.Value, 2 ) ) ) );
    }

    public bool ChangePosition( int delta )
    {
        //because JSON parser sets Position outside of the constructor, we have to take the latest value here instead
        if( _defaultPosition == Int32.MaxValue )
            _defaultPosition = Position.Value;

        Position.Value += delta;
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
