﻿public interface ISkill : IGameInit
{
    void Execute( UnitModel unitModel, SkillData skillData );
}