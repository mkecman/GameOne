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
        {
            //_unitController.RemoveUnit( unit );
            //return;
        }

        if( unit.Props[ R.Experience ].Delta < 4320 )
            unit.Props[ R.Experience ].Delta++;
    }

}
