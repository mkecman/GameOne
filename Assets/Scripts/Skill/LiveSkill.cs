using UnityEngine;

public class LiveSkill : ISkill
{
    public void Init(){}

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        ExecuteTime( 1, unitModel );
    }

    public void ExecuteTime( int time, UnitModel unit )
    {
        unit.Props[ R.Health ].Value -= time * ( 1 - unit.Props[ R.Armor ].Value );
        Mathf.Clamp( unit.Props[ R.Experience ].Delta += time, 0, 259200 );
    }
}
