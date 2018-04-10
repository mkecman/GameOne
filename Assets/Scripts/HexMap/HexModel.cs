using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class HexModel
{
    public int X, Y;
    
    internal UnitModel Unit;
    internal BuildingModel Building;

    public BoolReactiveProperty isMarked = new BoolReactiveProperty( false );
    public BoolReactiveProperty isExplored = new BoolReactiveProperty( false );

    public R Lens;

    public Dictionary<R,Resource> Props = new Dictionary<R,Resource>();

    public HexModel()
    {
        AddProp( R.Default );
        AddProp( R.Altitude, 2 );
        AddProp( R.Temperature );
        AddProp( R.Pressure );
        AddProp( R.Humidity );
        AddProp( R.Radiation );
        AddProp( R.HexScore );

        //AddProp( R.Energy );
        //AddProp( R.Science );
        //AddProp( R.Minerals );

        AddProp( R.Element, 118 );
    }

    private void AddProp( R prop, float maxValue = 1 )
    {
        Props[ prop ] = new Resource( prop, 0, 0, 0, maxValue );
    }
}
