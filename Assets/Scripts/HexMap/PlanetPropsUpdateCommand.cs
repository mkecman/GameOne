using PsiPhi;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlanetPropsUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private HexUpdateCommand _hexUpdateCommand;
    private PlanetModel _planet;
    private HexModel _hex;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
    }

    public void Execute()
    {
        _planet = _planetController.SelectedPlanet;
        for( int i = 0; i <= 100; i++ )
        {
            _planet.Props[ R.Temperature ].HexDistribution[ i ].Weight = 0f;
            _planet.Props[ R.Pressure ].HexDistribution[ i ].Weight = 0f;
            _planet.Props[ R.Humidity ].HexDistribution[ i ].Weight = 0f;
            _planet.Props[ R.Radiation ].HexDistribution[ i ].Weight = 0f;
        }

        for( int x = 0; x < _planet.Map.Width; x++ )
        {
            for( int y = 0; y < _planet.Map.Height; y++ )
            {
                _hex = _planet.Map.Table[ x ][ y ];
                _hexUpdateCommand.Execute( _hex );

                _planet.Props[ R.Temperature ].HexDistribution[ Mathf.FloorToInt( _hex.Props[ R.Temperature ].Value * 100 ) ].Weight += .05f;
                _planet.Props[ R.Pressure ].HexDistribution[ Mathf.FloorToInt( _hex.Props[ R.Pressure ].Value * 100 ) ].Weight += .05f;
                _planet.Props[ R.Humidity ].HexDistribution[ Mathf.FloorToInt( _hex.Props[ R.Humidity ].Value * 100 ) ].Weight += .05f;
                _planet.Props[ R.Radiation ].HexDistribution[ Mathf.FloorToInt( _hex.Props[ R.Radiation ].Value * 100 ) ].Weight += .05f;
            }
        }
    }
}
