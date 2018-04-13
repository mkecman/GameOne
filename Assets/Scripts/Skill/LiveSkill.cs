using UnityEngine;

public class LiveSkill : ISkill
{
    private PlanetController _planetController;
    private UnitController _unitController;
    private PlanetModel _planet;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitController = GameModel.Get<UnitController>();
    }

    public void Execute( UnitModel unitModel )
    {
        _planet = _planetController.SelectedPlanet;

        unitModel.Stats[ S.HP ].Value -= 1; //_planet.Map.Table[ unitModel.X ][ unitModel.Y ].Props[ R.HexScore ].Value;
        if( unitModel.Stats[ S.HP ].Value <= 0 )
            _unitController.RemoveUnit( unitModel );
    }
}
