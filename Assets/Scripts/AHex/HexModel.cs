using UnityEngine;
using System.Collections;
using UniRx;

public class HexModel
{
    public int X, Y;
    public float Altitude;
    public float Temperature;
    public Color Color;
    public ElementModel Element;
    public UnitModel Unit;

    public BoolReactiveProperty isMarked = new BoolReactiveProperty( false );

}
