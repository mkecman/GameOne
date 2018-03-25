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
                Position.y = (float)Props[ R.Altitude ].Value;
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
                Position.y = (float)Props[ R.Altitude ].Value;
                Position.z = HexMapHelper.GetZPosition( value );
                _Y.Value = value;
            }
    }
    
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public Dictionary<int, AbilityState> Abilities = new Dictionary<int, AbilityState>();

    public RDictionary<DoubleReactiveProperty> AbilitiesDelta = new RDictionary<DoubleReactiveProperty>( true );

    public RDictionary<BellCurve> Resistance = new RDictionary<BellCurve>();

    public RDictionary<Resource> Props = new RDictionary<Resource>();

    public Vector3 Position = new Vector3();

    public UnitModel( int x, int y, double altitude )
    {
        Props.Add( R.Altitude, new Resource( R.Altitude, altitude ) );
        Props.Add( R.Health, new Resource( R.Health, 100 ) );

        X = x;
        Y = y;

        Resistance.Add( R.Temperature, new BellCurve( 1, 0.33f, 0.06f ) );
        Resistance.Add( R.Pressure, new BellCurve( 1, 0.64f, 0.1f ) );
        Resistance.Add( R.Humidity, new BellCurve( 1, 1f, 0.2f ) );
        Resistance.Add( R.Radiation, new BellCurve( 1, 0f, 0.1f ) );

        //AbilitiesDelta[ R.Energy ].Value = -1;

        Abilities.Add( 0, AbilityState.UNLOCKED );
    }

    public UnitModel() { }
}
