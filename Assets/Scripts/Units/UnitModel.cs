using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class UnitModel
{
    public IntReactiveProperty X = new IntReactiveProperty();
    public IntReactiveProperty Y = new IntReactiveProperty();
    public DoubleReactiveProperty Altitude = new DoubleReactiveProperty();
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public Dictionary<int, AbilityState> Abilities = new Dictionary<int, AbilityState>();

    public RDictionary<double> AbilitiesDelta = new RDictionary<double>( true );

    public RDictionary<BellCurve> Resistance = new RDictionary<BellCurve>();
    
    public UnitModel( int x, int y, double altitude )
    {
        X.Value = x;
        Y.Value = y;
        Altitude.Value = altitude;

        Resistance[ R.Temperature] = new BellCurve( 1, 0.36f, 0.15f );
        Resistance[ R.Pressure]= new BellCurve( 1, 0.63f, 0.15f );
        Resistance[ R.Humidity]= new BellCurve( 1, 1f, 0.3f );
        Resistance[ R.Radiation]= new BellCurve( 1, 0f, 0.3f );

        AbilitiesDelta[ R.Energy ] = -2;

        Abilities.Add( 0, AbilityState.UNLOCKED );
    }

    public UnitModel() { }
}
