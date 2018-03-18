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

        Resistance[ R.Temperature] = new BellCurve( 1, 0.36f, 0.15f );
        Resistance[ R.Pressure]= new BellCurve( 1, 0.63f, 0.15f );
        Resistance[ R.Humidity]= new BellCurve( 1, 1f, 0.3f );
        Resistance[ R.Radiation]= new BellCurve( 1, 0f, 0.3f );

        AbilitiesDelta[ R.Energy ].Value = -2;

        Abilities.Add( 0, AbilityState.UNLOCKED );
    }

    public UnitModel() { }
}
