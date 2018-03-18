using UnityEngine;
using System.Collections;

public class PlanetHexTilesUpdate : ICommand
{
    PlanetController _planet;
    LifeController _life;
    UnitController _unit;

    public PlanetHexTilesUpdate()
    {
        _planet = GameModel.Get<PlanetController>();
        _life = GameModel.Get<LifeController>();
        _unit = GameModel.Get<UnitController>();
    }

    public void Execute( object[] data )
    {
        LifeModel life = _planet.SelectedPlanet.Life;
        _planet.Generate( _planet.SelectedPlanet.Index );
        _planet.SelectedPlanet.Life = life;
        _life.Load( _planet.SelectedPlanet );
        _unit.Load( _planet.SelectedPlanet );
        GameModel.Set( _planet.SelectedPlanet );
    }
}
