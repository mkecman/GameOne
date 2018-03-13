using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class HexModel
{
    public int X, Y;
    //public double Altitude;
    //public double Temperature;
    //public double Pressure;
    //public double Humidity;
    //public double Radiation;
    //public double TotalScore;
    //public ElementModel Element;
    public UnitModel Unit;

    public BoolReactiveProperty isMarked = new BoolReactiveProperty( false );
    public BoolReactiveProperty isExplored = new BoolReactiveProperty( false );

    public R Lens;

    public RDictionary<Resource> Props = new RDictionary<Resource>();

    public HexModel()
    {
        AddProp( R.Default );
        AddProp( R.Altitude );
        AddProp( R.Temperature );
        AddProp( R.Pressure );
        AddProp( R.Humidity );
        AddProp( R.Radiation );
        AddProp( R.HexScore );

        AddProp( R.Energy );
        AddProp( R.Science );
        AddProp( R.Minerals );
    }

    private void AddProp( R prop )
    {
        Props[ prop ] = new Resource( prop, 0 );
    }
}
