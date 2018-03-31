using UnityEngine;
using System.Collections;

public class PlanetGenerateCommand
{
    PlanetController _planet;
    LifeController _life;
    UnitController _unit;

    public PlanetGenerateCommand()
    {
        _planet = GameModel.Get<PlanetController>();
        _life = GameModel.Get<LifeController>();
        _unit = GameModel.Get<UnitController>();
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
        _life.Load( _planet.SelectedPlanet );
        GameModel.Set<PlanetModel>( _planet.SelectedPlanet );

        _unit.Load( _planet.SelectedPlanet );

    }
}
