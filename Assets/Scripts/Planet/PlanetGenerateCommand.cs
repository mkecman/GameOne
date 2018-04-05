﻿using UnityEngine;
using System.Collections;

public class PlanetGenerateCommand : IGameInit
{
    PlanetController _planet;

    public void Init()
    {
        _planet = GameModel.Get<PlanetController>();
    }

    public void Execute( HexMap map )
    {
        LifeModel life = _planet.SelectedPlanet.Life;
        //_planet.Generate( _planet.SelectedPlanet.Index );
        _planet.SelectedPlanet.Props[ R.Temperature ].Value = map.Temperature.Value;
        _planet.SelectedPlanet.Props[ R.Temperature ].Variation = map.TemperatureVariation.Value;

        _planet.SelectedPlanet.Props[ R.Pressure ].Value = map.Pressure.Value;
        _planet.SelectedPlanet.Props[ R.Pressure ].Variation = map.PressureVariation.Value;

        _planet.SelectedPlanet.Props[ R.Humidity ].Value = map.Humidity.Value;
        _planet.SelectedPlanet.Props[ R.Humidity ].Variation = map.HumidityVariation.Value;

        _planet.SelectedPlanet.Props[ R.Radiation ].Value = map.Radiation.Value;
        _planet.SelectedPlanet.Props[ R.Radiation ].Variation = map.RadiationVariation.Value;

        _planet.SelectedPlanet.LiquidLevel = map.LiquidLevel.Value;

        _planet.GenerateFromModel( _planet.SelectedPlanet );
        /////////
        _planet.SelectedPlanet.Life = life;

        GameModel.Get<PlanetPropsUpdateCommand>().Execute();
        GameModel.Set<PlanetModel>( _planet.SelectedPlanet );
    }
}
