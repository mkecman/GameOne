using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillPaymentService : AbstractController, IGameInit
{
    private UnitController _unitController;
    private CompoundConfig _compounds;
    private UnitModel _unit;

    public void Init()
    {
        _unitController = GameModel.Get<UnitController>();
        _compounds = GameConfig.Get<CompoundConfig>();
    }

}
