using System.Collections.Generic;

public class UnitUseCompoundCommand : IGameInit
{
    private CompoundConfig _compounds;
    private PlanetController _planetController;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _planetController = GameModel.Get<PlanetController>();
    }

    public void Execute( UnitModel unit, int compoundIndex )
    {
        if( _compounds[ compoundIndex ].Type == CompoundType.Consumable )
        {
            foreach( KeyValuePair<R, float> effect in _compounds[ compoundIndex ].Effects )
            {
                unit.Props[ effect.Key ].Value += effect.Value;
            }

            _planetController.SelectedPlanet.Life.Compounds[ compoundIndex ].Value--;
        }
    }

}
