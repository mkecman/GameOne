using UnityEngine;
using System.Collections;
using UniRx;

public class UnitModel
{
    public IntReactiveProperty X = new IntReactiveProperty();
    public IntReactiveProperty Y = new IntReactiveProperty();
    public DoubleReactiveProperty Altitude = new DoubleReactiveProperty();
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    [SerializeField]
    internal DoubleReactiveProperty _Energy = new DoubleReactiveProperty();
    public double Energy
    {
        get { return _Energy.Value; }
        set { _Energy.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _Science = new DoubleReactiveProperty();
    public double Science
    {
        get { return _Science.Value; }
        set { _Science.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _Reproduction = new DoubleReactiveProperty();
    public double Reproduction
    {
        get { return _Reproduction.Value; }
        set { _Reproduction.Value = value; }
    }

    public BellCurve TemperatureBC = new BellCurve( 1, 0.33f, 0.2f );
    public BellCurve PressureBC = new BellCurve( 1, 0.66f, 0.2f );
    public BellCurve HumidityBC = new BellCurve( 1, .85f, 0.1f );
    public BellCurve RadiationBC = new BellCurve( 1, 0.15f, 0.1f );
    
    public UnitModel( int x, int y, double altitude )
    {
        X.Value = x;
        Y.Value = y;
        Altitude.Value = altitude;
    }

    public UnitModel() { }
}
