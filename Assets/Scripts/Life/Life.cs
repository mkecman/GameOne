using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Life 
{
    private PlanetModel _planet;
    private LifeModel _life;

    private Units _units;
    private List<ElementModel> _elements;

    public Life()
    {
        _elements = Config.Get<ElementConfig>().Elements;
        _units = new Units();
    }
    
    public LifeModel New( PlanetModel planet )
    {
        _planet = planet;
        _life = new LifeModel
        {
            Name = "Human",
            Population = 1,
            Science = 4
        };
        _planet.Life = _life;

        Load( _planet );
        
        return _life;
    }

    internal void Load( PlanetModel planet )
    {
        _planet = planet;
        _life = _planet.Life;
        _units.Load( planet );
        UpdatePlanetMapColors();
    }
    
    public void UpdateStep( int steps )
    {
        for( uint i = 0; i < steps; i++ )
        {
            _units.UpdateStep();
        }
        CSV.Add( _life.Population + "," + _life.Science + "," + _life.Words );
    }

    private void UpdatePlanetMapColors()
    {
        HexModel hex;
        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                hex = _planet.Map.Table[ x, y ];

                float temperatureBonus = _life.TemperatureBC.GetValueAt( hex.Temperature );
                float pressureBonus = _life.PressureBC.GetValueAt( hex.Pressure );
                float humidityBonus = _life.HumidityBC.GetValueAt( hex.Humidity );
                float radiationBonus = _life.RadiationBC.GetValueAt( hex.Radiation );
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
