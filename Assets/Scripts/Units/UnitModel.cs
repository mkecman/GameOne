using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class UnitModel
{
    public IntReactiveProperty X = new IntReactiveProperty();
    public IntReactiveProperty Y = new IntReactiveProperty();
    public DoubleReactiveProperty Altitude = new DoubleReactiveProperty();
    public BoolReactiveProperty isSelected = new BoolReactiveProperty( false );

    public DoubleReactiveProperty EnergyConsumption = new DoubleReactiveProperty();

    public List<Ability> ActiveAbilities = new List<Ability>();
    public List<Ability> UnlockedAbilities = new List<Ability>();

    public RDictionary<double> AbilitiesDelta = new RDictionary<double>();

    public RDictionary<BellCurve> Resistance = new RDictionary<BellCurve>();
    
    public UnitModel( int x, int y, double altitude )
    {
        X.Value = x;
        Y.Value = y;
        Altitude.Value = altitude;

        Resistance.Set( R.Temperature, new BellCurve( 1, 0.36f, 0.15f ) );
        Resistance.Set( R.Pressure, new BellCurve( 1, 0.63f, 0.15f ) );
        Resistance.Set( R.Humidity, new BellCurve( 1, 1f, 0.3f ) );
        Resistance.Set( R.Radiation, new BellCurve( 1, 0f, 0.3f ) );

        AbilitiesDelta.Set( R.Energy, -2 );
    }

    public UnitModel() { }
}
