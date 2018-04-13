using System.Collections.Generic;

public class SkillCommand : IGameInit
{
    private Dictionary<SkillName, ISkill> _skills;

    public void Init()
    {
        _skills = new Dictionary<SkillName, ISkill>
        {
            { SkillName.Live, GameModel.Get<LiveSkill>() },
            { SkillName.Clone, GameModel.Get<CloneSkill>() },
            { SkillName.Move, GameModel.Get<MoveSkill>() },
            { SkillName.Mine, GameModel.Get<MineSkill>() }
        };
    }

    internal void Execute( UnitModel um, SkillName skillName )
    {
        _skills[ skillName ].Execute( um );
    }
}
