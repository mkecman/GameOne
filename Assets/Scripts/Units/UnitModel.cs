using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class UnitModel
{
    [SerializeField]
    internal IntReactiveProperty _X = new IntReactiveProperty();
    public int X
    {
        get { return _X.Value; }
        set {
                Position.x = HexMapHelper.GetXPosition( value, Y );
                Position.y = Props[ R.Altitude ].Value;
                _X.Value = value;
            }
    }

    [SerializeField]
    internal IntReactiveProperty _Y = new IntReactiveProperty();
    public int Y
    {
        get { return _Y.Value; }
        set {
                Position.x = HexMapHelper.GetXPosition( X, value );
                Position.y = Props[ R.Altitude ].Value;
                Position.z = HexMapHelper.GetZPosition( value );
                _Y.Value = value;
            }
    }
    
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public Dictionary<int, BuildingState> Abilities = new Dictionary<int, BuildingState>();

    public Dictionary<R, FloatReactiveProperty> AbilitiesDelta = new Dictionary<R, FloatReactiveProperty>();

    public Dictionary<R, BellCurve> Resistance = new Dictionary<R,BellCurve>();

    public Dictionary<R,Resource> Props = new Dictionary<R,Resource>();

    internal Vector3 Position = new Vector3();

    public UnitModel( int x, int y, float altitude, Dictionary<R, BellCurve> resistance )
    {
        Props.Add( R.Altitude, new Resource( R.Altitude, altitude ) );
        Props.Add( R.Health, new Resource( R.Health, 100 ) );

        X = x;
        Y = y;

        Resistance = resistance;
        
        AbilitiesDelta.Add( R.Energy, new FloatReactiveProperty( -.5f) );
        AbilitiesDelta.Add( R.Science, new FloatReactiveProperty() );
        AbilitiesDelta.Add( R.Minerals, new FloatReactiveProperty() );

        Abilities.Add( 0, BuildingState.UNLOCKED );
    }

    public UnitModel() { }
}
