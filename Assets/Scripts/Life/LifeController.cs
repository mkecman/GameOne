using System;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : AbstractController
{
    public LifeModel SelectedLife { get { return _selectedLife; } }

    private PlanetModel _planet;
    private LifeModel _selectedLife;
    private HexUpdateCommand _hexUpdateCommand;

    public LifeController()
    {
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
    }

    public void New( PlanetModel planet )
    {
        _planet = planet;
        _selectedLife = new LifeModel
        {
            Name = "Human",
            ClimbLevel = 0.99
        };
        _selectedLife.Props[ R.Energy ].Value = 500;
        _selectedLife.Props[ R.Science ].Value = 500;
        _selectedLife.Props[ R.Population ].Value = 1;
        _selectedLife.BuildingsState = GameModel.Copy( Config.Get<BuildingConfig>().Buildings );

        int unitX = (int)( _planet.Map.Width / 2 ) + 2;
        int unitY = (int)( _planet.Map.Height / 2 ) + 2;
        _selectedLife.Units.Add( new UnitModel( unitX, unitY, _planet.Map.Table[ unitX, unitY ].Props[ R.Altitude ].Value ) );

        _planet.Life = _selectedLife;
        UpdatePlanetMapColors();
    }

    public void Load( PlanetModel planet )
    {
        _planet = planet;
        _selectedLife = _planet.Life;
        UpdatePlanetMapColors();
    }
    

    private void UpdatePlanetMapColors()
    {
        HexModel hex;

        int[] foodTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] scienceTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        int[] wordsTiles = new int[] { 0, 0, 0, 0, 0, 0 };
        double totalEnergy = 0;
        double totalScience = 0;
        double totalMinerals = 0;

        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x, y ];

                _hexUpdateCommand.Execute( _selectedLife.Resistance, hex );

                totalEnergy += hex.Props[ R.Energy ].Value;
                totalScience += hex.Props[ R.Science ].Value;
                totalMinerals += hex.Props[ R.Minerals ].Value;

                foodTiles[ (int)hex.Props[ R.Energy ].Value ]++;
                scienceTiles[ (int)hex.Props[ R.Science ].Value ]++;
                wordsTiles[ (int)hex.Props[ R.Minerals ].Value ]++;
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
