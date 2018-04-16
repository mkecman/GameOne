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

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        _planet = _planetController.SelectedPlanet;

        unitModel.Props[ R.Health ].Value -= 1; //_planet.Map.Table[ unitModel.X ][ unitModel.Y ].Props[ R.HexScore ].Value;
        if( unitModel.Props[ R.Health ].Value <= 0 )
            _unitController.RemoveUnit( unitModel );
    }
}
