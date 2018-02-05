using UnityEngine;
using System.Collections;
using UniRx;

public class UnitModel
{
    public int X;
    public int Y;
    public int MoveRange = 1;
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public UnitModel( int x, int y )
    {
        this.X = x;
        this.Y = y;
    }
}
