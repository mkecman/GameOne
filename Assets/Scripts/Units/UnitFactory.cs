using System.Collections.Generic;

public class UnitFactory : IGameInit
{
    private PlanetController _planetController;
    private BellCurveConfig _bellCurves;
    private SkillConfig _skills;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _skills = GameConfig.Get<SkillConfig>();
    }

    public UnitModel GetUnit( int x, int y )
    {
        UnitModel unit = new UnitModel();

        //STATS
        unit.Props.Add( R.Health, new Resource( R.Health, 100, 0, 0, 100 ) );
        unit.Props.Add( R.Experience, new Resource( R.Experience, 1, 0, 0, 32000 ) );
        unit.Props.Add( R.Level, new Resource( R.Level, 1, 0, 1, 100 ) );
        unit.Props.Add( R.UpgradePoint, new Resource( R.UpgradePoint, 0, 0, 0, 100 ) );

        unit.Props.Add( R.Body, new Resource( R.Body, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Mind, new Resource( R.Mind, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Soul, new Resource( R.Soul, 1, 0, 0, 100 ) );

        unit.Props.Add( R.Armor, new Resource( R.Armor, 0, 0, 0, 100 ) );
        unit.Props.Add( R.Attack, new Resource( R.Attack, 0, 0, 0, 100 ) );
        unit.Props.Add( R.Speed, new Resource( R.Speed, 1, 0, 0, 100 ) );
        unit.Props.Add( R.Critical, new Resource( R.Critical, 0, 0, 0, 100 ) );

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

        unit.Setup();

        return unit;
    }
}
