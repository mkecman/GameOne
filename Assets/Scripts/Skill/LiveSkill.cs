using UnityEngine;

public class LiveSkill : ISkill
{
    private PlanetController _planetController;
    private UnitController _unitController;
    private PlanetModel _planet;
    private HexModel _hex;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitController = GameModel.Get<UnitController>();
    }

    public void Execute( UnitModel unit, SkillData skillData )
    {
        _planet = _planetController.SelectedPlanet;
        _hex = _planet.Map.Table[ unit.X ][ unit.Y ];

        unit.Props[ R.Health ].Value -= 1 - unit.Props[ R.Armor ].Value;

        if( unit.Props[ R.Health ].Value <= 0 )
            _unitController.RemoveUnit( unit );
    }

}
