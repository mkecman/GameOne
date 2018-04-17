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

        unit.Props.Add( R.Altitude, new Resource( R.Altitude, _planetController.SelectedPlanet.Map.Table[ x ][ y ].Props[ R.Altitude ].Value, 0, 0, 2 ) );
        unit.Props.Add( R.Health, new Resource( R.Health, 100, 0, 0, 100 ) );

        unit.X = x;
        unit.Y = y;

        unit.Resistance = GameModel.Copy( _bellCurves );

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
}
