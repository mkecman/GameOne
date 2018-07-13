using System;

public class UnitFactory : IGameInit
{
    private PlanetModel _planet;
    private BellCurveConfig _bellCurves;
    private SkillConfig _skills;
    private UnitTypesConfig _unitsConfig;
    private CompoundTreeConfig _compoundTree;

    public void Init()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _skills = GameConfig.Get<SkillConfig>();
        _unitsConfig = GameConfig.Get<UnitTypesConfig>();
        _compoundTree = GameConfig.Get<CompoundTreeConfig>();
    }

    public UnitModel GetUnitType( int index, int x, int y )
    {
        UnitModel unit = GetUnit( x, y );

        unit.Impact[ R.Temperature ].Value = _unitsConfig[ index ].ITemperature;
        _planet.Impact[ R.Temperature ].Value += _unitsConfig[ index ].ITemperature;
        unit.Impact[ R.Pressure ].Value = _unitsConfig[ index ].IPressure;
        _planet.Impact[ R.Pressure ].Value += _unitsConfig[ index ].IPressure;
        unit.Impact[ R.Humidity ].Value = _unitsConfig[ index ].IHumidity;
        _planet.Impact[ R.Humidity ].Value += _unitsConfig[ index ].IHumidity;
        //unit.Impact[ R.Radiation ].Value = _unitsConfig[ index ].IRadiation;
        //_planet.Impact[ R.Radiation ].Value += _unitsConfig[ index ].IRadiation;

        unit.Resistance[ R.Temperature ].Position.Value = _unitsConfig[ index ].Temperature;
        unit.Resistance[ R.Pressure ].Position.Value = _unitsConfig[ index ].Pressure;
        unit.Resistance[ R.Humidity ].Position.Value = _unitsConfig[ index ].Humidity;
        unit.Resistance[ R.Radiation ].Position.Value = _unitsConfig[ index ].Radiation;

        unit.Props[ R.Mind ].Value = _unitsConfig[ index ].Mind;
        unit.Props[ R.Soul ].Value = _unitsConfig[ index ].Soul;
        unit.Props[ R.Body ].Value = _unitsConfig[ index ].Body;

        unit.Setup();
        unit.Props[ R.Health ].Value = unit.Props[ R.Health ].MaxValue;

        return unit;
    }

    private UnitModel GetUnit( int x, int y )
    {
        UnitModel unit = new UnitModel();

        //STATS
        unit.Props.Add( R.Health, new Resource( R.Health, 100, 0, 0, 100 ) );
        unit.Props.Add( R.Experience, new Resource( R.Experience, 0, 0, 0, 1 ) );
        unit.Props.Add( R.Level, new Resource( R.Level, 1, 0, 1, 100 ) );
        unit.Props.Add( R.UpgradePoint, new Resource( R.UpgradePoint, 0, 0, 0, 100 ) );

        unit.Props.Add( R.Body, new Resource( R.Body, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Mind, new Resource( R.Mind, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Soul, new Resource( R.Soul, 1, 0, 0, 100 ) );

        unit.Props.Add( R.Armor, new Resource( R.Armor, 0, 0, 0, 100 ) );
        unit.Props.Add( R.Attack, new Resource( R.Attack, 0, 0, 0, 1000 ) );
        unit.Props.Add( R.Speed, new Resource( R.Speed, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Critical, new Resource( R.Critical, 0, 0, 0, 100 ) );

        unit.Resistance = GameModel.Copy( _bellCurves );

        //MAP POSITION
        unit.Props.Add( R.Altitude, new Resource( R.Altitude, _planet.Map.Table[ x ][ y ].Props[ R.Altitude ].Value, 0, 0, 2 ) );
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

        //ORGAN TREE
        unit.OrganTree = GameModel.Copy( _compoundTree );

        return unit;
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
    }
}
