using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SkillController : IGameInit
{
    private UnitModel _unit;
    private SkillConfig _skills;
    private SkillMessage _skillMessage = new SkillMessage();

    public void Init()
    {
        _skills = GameConfig.Get<SkillConfig>();
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        GameMessage.Listen<SkillControlMessage>( OnSkillControlMessage );
    }

    private void OnSkillControlMessage( SkillControlMessage value )
    {
        if( _unit != null ) //shouldn't happen actually!
        {
            if( value.State == SkillState.UNLOCKED )
                UnlockSkill( value.Index );
            if( value.State == SkillState.SELECTED )
                SelectSkill( value.Index );
        }
    }

    private SkillData _skill;

    private void SelectSkill( int index )
    {
        _skill = _unit.Skills[ index ];

        //remove previously selected skill
        foreach( KeyValuePair<int, SkillData> item in _unit.Skills )
        {
            if( _skill.Type == item.Value.Type && item.Value.State == SkillState.SELECTED )
            {
                item.Value.State = SkillState.UNLOCKED;
                if( item.Value.IsPassive )
                    _unit.PassiveSkills.Remove( item.Value.Index );
                else
                    _unit.ActiveSkills.Remove( item.Value.Index );

                SendSkillMessage( item.Value.Index, SkillState.UNLOCKED );
                break;
            }
        }
        
        _skill.State = SkillState.SELECTED;
        if( _skill.IsPassive )
            _unit.PassiveSkills.Add( index );
        else
            _unit.ActiveSkills.Add( index );

        SendSkillMessage( index, SkillState.SELECTED );
    }

    private void SendSkillMessage( int index, SkillState state )
    {
        _skillMessage.Index = index;
        _skillMessage.State = state;
        GameMessage.Send( _skillMessage );
    }

    private void UnlockSkill( int index )
    {
        _unit.Skills.Add( index, GameModel.Copy( _skills[ index ] ) );
        _unit.Skills[ index ].State = SkillState.UNLOCKED;

        SendSkillMessage( index, SkillState.UNLOCKED );
    }

    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
    }
}
