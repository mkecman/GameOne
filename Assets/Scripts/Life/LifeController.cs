using PsiPhi;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : AbstractController, IGameInit
{
    public LifeModel SelectedLife { get { return _selectedLife; } }

    private PlanetModel _planet;
    private LifeModel _selectedLife;
    private Dictionary<R, BellCurve> _bellCurves;
    private HexUpdateCommand _hexUpdateCommand;

    public void Init()
    {
        _bellCurves = GameConfig.Get<BellCurveConfig>();
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
        _selectedLife = _planet.Life;
        if( _selectedLife.Resistance.Count > 0 )
            UpdatePlanetMapColors();
    }

    public void New()
    {
        _selectedLife.Name = "Human";
        _selectedLife.ClimbLevel = 0.99;

        _selectedLife.Props[ R.Energy ].Value = 500;
        _selectedLife.Props[ R.Science ].Value = 500;
        _selectedLife.Props[ R.Minerals ].Value = 500;
        _selectedLife.Props[ R.Population ].Value = 1;
        _selectedLife.BuildingsState = GameModel.Copy( GameConfig.Get<BuildingConfig>().Buildings );
        _selectedLife.Resistance = GameModel.Copy( _bellCurves );

        int unitX = (int)( _planet.Map.Width / 2 ) + 2;
        int unitY = (int)( _planet.Map.Height / 2 ) + 2;
        _selectedLife.Units.Add( new UnitModel( unitX, unitY, _planet.Map.Table[ unitX ][ unitY ].Props[ R.Altitude ].Value, GameModel.Copy( _bellCurves ) ) );

        _planet.Life = _selectedLife;
        UpdatePlanetMapColors();
    }

    private void UpdatePlanetMapColors()
    {
        HexModel hex;

        int[] foodTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] scienceTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] wordsTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        float totalEnergy = 0;
        float totalScience = 0;
        float totalMinerals = 0;
        
        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x ][ y ];

                _hexUpdateCommand.Execute( _selectedLife.Resistance, hex );

                totalEnergy += hex.Props[ R.Energy ].Value;
                totalScience += hex.Props[ R.Science ].Value;
                totalMinerals += hex.Props[ R.Minerals ].Value;

                foodTiles[ (int)hex.Props[ R.Energy ].Value ]++;
                scienceTiles[ (int)hex.Props[ R.Science ].Value ]++;
                //wordsTiles[ (int)hex.Props[ R.Minerals ].Value ]++;

                
            }
        }
        
        Debug.Log( "Energy: " + totalEnergy + "::: Science: " + totalScience + "::: Minerals: " + totalMinerals );

        for( int i = 0; i < 6; i++ )
        {
            totalEnergy += i * foodTiles[ i ];
            totalScience += i * scienceTiles[ i ];
            totalMinerals += i * wordsTiles[ i ];
        }

    }
}
