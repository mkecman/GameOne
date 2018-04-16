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

        unit.Skills.Add( SkillType.Live, GameModel.Copy( _skills[ SkillType.Live ][ 0 ] ) );
        unit.Skills.Add( SkillType.Mine, GameModel.Copy( _skills[ SkillType.Mine ][ 0 ] ) );
        unit.Skills.Add( SkillType.Move, GameModel.Copy( _skills[ SkillType.Move ][ 0 ] ) );
        unit.Skills.Add( SkillType.Clone, GameModel.Copy( _skills[ SkillType.Clone ][ 0 ] ) );

        unit.ActiveSkills.Add( SkillType.Live );
        unit.ActiveSkills.Add( SkillType.Mine );

        unit.PassiveSkills.Add( SkillType.Move );
        unit.PassiveSkills.Add( SkillType.Clone );

        return unit;
    }
}
