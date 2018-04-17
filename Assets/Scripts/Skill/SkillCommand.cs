using System.Collections.Generic;

public class SkillCommand : IGameInit
{
    private Dictionary<SkillType, ISkill> _skills;

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

    internal void Execute( UnitModel um, SkillType type )
    {
        _skills[ type ].Execute( um, um.Skills[ type ] );
    }
}
