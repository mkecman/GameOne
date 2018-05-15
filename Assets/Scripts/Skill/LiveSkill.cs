using UnityEngine;

public class LiveSkill : ISkill
{
    private UnitController _unitController;

    public void Init()
    {
        _unitController = GameModel.Get<UnitController>();
    }

    public void Execute( UnitModel unit, SkillData skillData )
    {
        unit.Props[ R.Health ].Value -= 1 - unit.Props[ R.Armor ].Value;

        if( unit.Props[ R.Health ].Value <= 0 )
            unit.Props[ R.Health ].Value = unit.Props[ R.Health ].MaxValue;
            //_unitController.RemoveUnit( unit );
    }

}
