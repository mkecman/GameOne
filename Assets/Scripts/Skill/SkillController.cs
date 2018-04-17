using UnityEngine;
using System.Collections;
using System;

public class SkillController : IGameInit
{
    private UnitModel _unit;

    public void Init()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
    }
}
