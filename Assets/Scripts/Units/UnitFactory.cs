public class UnitFactory : IGameInit
{
    private PlanetController _planetController;
    private BellCurveConfig _bellCurves;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _bellCurves = GameConfig.Get<BellCurveConfig>();
    }

    public UnitModel GetUnit( int x, int y )
    {
        UnitModel unit = new UnitModel();

        unit.Props.Add( R.Altitude, new Resource( R.Altitude, _planetController.SelectedPlanet.Map.Table[ x ][ y ].Props[ R.Altitude ].Value, 0, 0, 2 ) );
        unit.Props.Add( R.Health, new Resource( R.Health, 100, 0, 0, 100 ) );

        unit.X = x;
        unit.Y = y;

        unit.Resistance = GameModel.Copy( _bellCurves );

        for( int i = 0; i < (int)S.Count; i++ )
            unit.Stats.Add( (S)i, new ResourceInt( (S)i, 1 ) );

        return unit;
    }
}
