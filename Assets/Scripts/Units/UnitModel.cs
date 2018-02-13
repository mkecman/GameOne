using UnityEngine;
using System.Collections;
using UniRx;

public class UnitModel
{
    public IntReactiveProperty X = new IntReactiveProperty();
    public IntReactiveProperty Y = new IntReactiveProperty();
    public DoubleReactiveProperty Altitude = new DoubleReactiveProperty();
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public UnitModel( int x, int y, double altitude )
    {
        X.Value = x;
        Y.Value = y;
        Altitude.Value = altitude;
    }

    public UnitModel() { }
}
