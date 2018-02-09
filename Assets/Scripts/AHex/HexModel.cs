using UnityEngine;
using System.Collections;
using UniRx;

public class HexModel
{
    public int X, Y;
    public float Altitude;
    public float Temperature;
    public float Pressure;
    public float Humidity;
    public float Radiation;
    public float TotalScore;
    public Color[] Colors = new Color[7];
    public ElementModel Element;
    public UnitModel Unit;

    public BoolReactiveProperty isMarked = new BoolReactiveProperty( false );

    public HexMapLens Lens;

}
