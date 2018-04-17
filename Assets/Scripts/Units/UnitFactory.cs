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

        unit.Skills.Add( SkillType.LIVE, GameModel.Copy( _skills[ SkillType.LIVE ][ 0 ] ) );
        unit.Skills.Add( SkillType.MINE, GameModel.Copy( _skills[ SkillType.MINE ][ 0 ] ) );
        unit.Skills.Add( SkillType.MOVE, GameModel.Copy( _skills[ SkillType.MOVE ][ 0 ] ) );
        unit.Skills.Add( SkillType.CLONE, GameModel.Copy( _skills[ SkillType.CLONE ][ 0 ] ) );

        unit.ActiveSkills.Add( SkillType.LIVE );
        unit.ActiveSkills.Add( SkillType.MINE );

        unit.PassiveSkills.Add( SkillType.MOVE );
        unit.PassiveSkills.Add( SkillType.CLONE );

        return unit;
    }
}
