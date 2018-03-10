using System;
using UnityEngine;

public class LifeController : AbstractController
{
    public LifeModel SelectedLife { get { return _selectedLife; } }

    private PlanetModel _planet;
    private LifeModel _selectedLife;

    public void New( PlanetModel planet )
    {
        _planet = planet;
        _selectedLife = new LifeModel
        {
            Name = "Human",
            ClimbLevel = 0.18
        };
        _selectedLife.Props[ R.Energy ].Value = 500;
        _selectedLife.Props[ R.Population ].Value = 1;

        int unitX = (int)( _planet.Map.Width / 2 );
        int unitY = (int)( _planet.Map.Height / 2 );
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


        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x, y ];

                float temperatureBonus = _selectedLife.Resistance[ R.Temperature ].GetValueAt( hex.Props[ R.Temperature ].Value );
                float pressureBonus = _selectedLife.Resistance[ R.Pressure ].GetValueAt( hex.Props[ R.Pressure ].Value );
                float humidityBonus = _selectedLife.Resistance[ R.Humidity ].GetValueAt( hex.Props[ R.Humidity ].Value );
                float radiationBonus = _selectedLife.Resistance[ R.Radiation ].GetValueAt( hex.Props[ R.Radiation ].Value );

                hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, temperatureBonus );
                hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, pressureBonus );
                hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, humidityBonus );
                hex.Props[ R.Radiation ].Color = Color.Lerp( Color.red, Color.green, radiationBonus );

                hex.Props[ R.Energy ].Value = Math.Round( ( temperatureBonus + humidityBonus ) * 1.74, 0 );
                hex.Props[ R.Energy ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Energy ].Value / 10 );

                hex.Props[ R.Science ].Value = Math.Round( ( ( 1 - temperatureBonus ) + ( 1 - radiationBonus ) ) * 1.74, 0 );
                hex.Props[ R.Science ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Science ].Value / 10 );

                hex.Props[ R.Minerals ].Value = Math.Round( ( temperatureBonus + radiationBonus ) * 1.74, 0 );
                hex.Props[ R.Minerals ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.Minerals ].Value / 10 );

                //hex.TotalScore = Math.Round( ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4, 2 );
                hex.Props[ R.HexScore ].Value = Math.Round( ( hex.Props[ R.Energy ].Value + hex.Props[ R.Science ].Value + hex.Props[ R.Minerals ].Value ) / 10, 2 );
                hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, (float)hex.Props[ R.HexScore ].Value );

                foodTiles[ (int)hex.Props[ R.Energy ].Value ]++;
                scienceTiles[ (int)hex.Props[ R.Science ].Value ]++;
                wordsTiles[ (int)hex.Props[ R.Minerals ].Value ]++;
            }
        }

        double totalEnergy = 0;
        double totalScience = 0;
        double totalMinerals = 0;
        for( int i = 0; i < 6; i++ )
        {
            totalEnergy += i * foodTiles[ i ];
            totalScience += i * scienceTiles[ i ];
            totalMinerals += i * wordsTiles[ i ];
        }

        Debug.Log( "Energy: " + totalEnergy + "::: Science: " + totalScience + "::: Minerals: " + totalMinerals );
    }
}
