using System.Collections.Generic;
using UnityEngine;

public class SkillCommand : IGameInit
{
    private Dictionary<SkillType, ISkill> _skills;
    private SkillData _skill;

    public void Init()
    {
        _skills = new Dictionary<SkillType, ISkill>
        {
            { SkillType.LIVE, GameModel.Get<LiveSkill>() },
            { SkillType.CLONE, GameModel.Get<CloneSkill>() },
            { SkillType.MOVE, GameModel.Get<MoveSkill>() },
            { SkillType.MINE, GameModel.Get<MineSkill>() }
        };
    }

    internal void Execute( UnitModel um, int index )
    {
        _skill = um.Skills[ index ];
        _skills[ _skill.Type ].Execute( um, _skill );
    }
}
