using UnityEngine;
using System.Collections;
using UniRx;

public class HexModel
{
    public int X, Y;
    public double Altitude;
    public double Temperature;
    public double Pressure;
    public double Humidity;
    public double Radiation;
    public double TotalScore;
    public Color[] Colors = new Color[7];
    public ElementModel Element;
    public UnitModel Unit;

    public BoolReactiveProperty isMarked = new BoolReactiveProperty( false );
    public BoolReactiveProperty isExplored = new BoolReactiveProperty( false );

    public HexMapLens Lens;

}
