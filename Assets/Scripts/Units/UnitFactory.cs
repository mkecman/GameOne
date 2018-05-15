using System.Collections.Generic;

public class UnitFactory : IGameInit
{
    private PlanetController _planetController;
    private BellCurveConfig _bellCurves;
    private SkillConfig _skills;
    private UnitTypesConfig _unitsConfig;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _skills = GameConfig.Get<SkillConfig>();
        _unitsConfig = GameConfig.Get<UnitTypesConfig>();
    }

    public UnitModel GetUnit( int x, int y )
    {
        UnitModel unit = new UnitModel();

        //STATS
        unit.Props.Add( R.Health, new Resource( R.Health, 100, 0, 0, 100 ) );
        unit.Props.Add( R.Experience, new Resource( R.Experience, 1, 0, 0, 32000 ) );
        unit.Props.Add( R.Level, new Resource( R.Level, 1, 0, 1, 100 ) );
        unit.Props.Add( R.Body, new Resource( R.Body, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Mind, new Resource( R.Mind, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Soul, new Resource( R.Soul, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Speed, new Resource( R.Speed, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Armor, new Resource( R.Armor, 0, 0, 0, 100 ) );
        unit.Props.Add( R.Attack, new Resource( R.Attack, 0, 0, 0, 100 ) );
        unit.Props.Add( R.UpgradePoint, new Resource( R.UpgradePoint, 0, 0, 0, 100 ) );

        //PLANET INFLUENCE
        unit.Props.Add( R.Temperature, new Resource( R.Temperature, 2, 0, -10, 10 ) );
        unit.Props.Add( R.Pressure, new Resource( R.Pressure, 1, 0, -10, 10 ) );
        unit.Props.Add( R.Humidity, new Resource( R.Humidity, -1, 0, -10, 10 ) );
        unit.Props.Add( R.Radiation, new Resource( R.Radiation, -2, 0, -10, 10 ) );

        unit.Resistance = GameModel.Copy( _bellCurves );

        //MAP POSITION
        unit.Props.Add( R.Altitude, new Resource( R.Altitude, _planetController.SelectedPlanet.Map.Table[ x ][ y ].Props[ R.Altitude ].Value, 0, 0, 2 ) );
        //unit.Props.Add( R.Altitude, new Resource( R.Altitude, 0 ) );
        unit.X = x;
        unit.Y = y;

        //SKILLS
        unit.Skills.Add( 0, GameModel.Copy( _skills[ 0 ] ) ); //live
        unit.Skills[ 0 ].State = SkillState.SELECTED;
        unit.Skills.Add( 1, GameModel.Copy( _skills[ 1 ] ) ); //clone
        unit.Skills[ 1 ].State = SkillState.SELECTED;
        unit.Skills.Add( 2, GameModel.Copy( _skills[ 2 ] ) ); //craft
        unit.Skills[ 2 ].State = SkillState.SELECTED;
        unit.Skills.Add( 3, GameModel.Copy( _skills[ 3 ] ) ); //move
        unit.Skills[ 3 ].State = SkillState.SELECTED;
        unit.Skills.Add( 4, GameModel.Copy( _skills[ 4 ] ) ); //mine
        unit.Skills[ 4 ].State = SkillState.SELECTED;

        unit.PassiveSkills.Add( 0 );//live
        unit.PassiveSkills.Add( 4 ); //mine

        unit.ActiveSkills.Add( 3 );//move
        unit.ActiveSkills.Add( 1 );//clone

        return unit;
    }

    public UnitModel GetUnitType( int index, int x, int y )
    {
        UnitModel unit = GetUnit( x, y );

        unit.Props[ R.Temperature ].Value = _unitsConfig[ index ].ITemperature;
        unit.Props[ R.Pressure ].Value = _unitsConfig[ index ].IPressure;
        unit.Props[ R.Humidity ].Value = _unitsConfig[ index ].IHumidity;
        unit.Props[ R.Radiation ].Value = _unitsConfig[ index ].IRadiation;

        unit.Resistance[ R.Temperature ].Position.Value = _unitsConfig[ index ].Temperature;
        unit.Resistance[ R.Pressure ].Position.Value = _unitsConfig[ index ].Pressure;
        unit.Resistance[ R.Humidity ].Position.Value = _unitsConfig[ index ].Humidity;
        unit.Resistance[ R.Radiation ].Position.Value = _unitsConfig[ index ].Radiation;

        unit.Props[ R.Mind ].Value = _unitsConfig[ index ].Mind;
        unit.Props[ R.Soul ].Value = _unitsConfig[ index ].Soul;
        unit.Props[ R.Body ].Value = _unitsConfig[ index ].Body;

        unit.Setup();
        return unit;
    }
}
