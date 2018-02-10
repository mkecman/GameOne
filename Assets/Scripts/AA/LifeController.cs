using UnityEngine;
using System.Collections;
using System;

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
            Population = 1,
            Science = 4
        };
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
        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x, y ];

                float temperatureBonus = _selectedLife.TemperatureBC.GetValueAt( hex.Temperature );
                float pressureBonus = _selectedLife.PressureBC.GetValueAt( hex.Pressure );
                float humidityBonus = _selectedLife.HumidityBC.GetValueAt( hex.Humidity );
                float radiationBonus = _selectedLife.RadiationBC.GetValueAt( hex.Radiation );
                hex.Colors[ (int)HexMapLens.Temperature ] = Color.Lerp( Color.red, Color.green, temperatureBonus );
                hex.Colors[ (int)HexMapLens.Pressure ] = Color.Lerp( Color.red, Color.green, pressureBonus );
                hex.Colors[ (int)HexMapLens.Humidity ] = Color.Lerp( Color.red, Color.green, humidityBonus );
                hex.Colors[ (int)HexMapLens.Radiation ] = Color.Lerp( Color.red, Color.green, radiationBonus );

                hex.TotalScore = ( temperatureBonus + pressureBonus + humidityBonus + radiationBonus ) / 4;
                hex.Colors[ (int)HexMapLens.TotalScore ] = Color.Lerp( Color.red, Color.green, hex.TotalScore );
            }
        }
    }
}
