using UnityEngine;
using System.Collections;
using UniRx;

public class UnitModel
{
    public IntReactiveProperty X = new IntReactiveProperty();
    public IntReactiveProperty Y = new IntReactiveProperty();
    public FloatReactiveProperty Altitude = new FloatReactiveProperty();
    public int MoveRange = 1;
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public UnitModel( int x, int y, float altitude )
    {
        X.Value = x;
        Y.Value = y;
        Altitude.Value = altitude;
    }
}
