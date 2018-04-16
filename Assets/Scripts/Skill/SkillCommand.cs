using System.Collections.Generic;

public class SkillCommand : IGameInit
{
    private Dictionary<SkillType, ISkill> _skills;

    public void Init()
    {
        _skills = new Dictionary<SkillType, ISkill>
        {
            { SkillType.Live, GameModel.Get<LiveSkill>() },
            { SkillType.Clone, GameModel.Get<CloneSkill>() },
            { SkillType.Move, GameModel.Get<MoveSkill>() },
            { SkillType.Mine, GameModel.Get<MineSkill>() }
        };
    }

    internal void Execute( UnitModel um, SkillType type )
    {
        _skills[ type ].Execute( um, um.Skills[ type ] );
    }
}
