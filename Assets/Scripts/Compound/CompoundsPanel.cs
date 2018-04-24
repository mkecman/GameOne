using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UniRx;

public class CompoundsPanel : GameView
{
    public Transform Container;
    public GameObject SkillUnlockPrefab;
    public GameObject EffectPrefab;

    private BuildingMessage _abilityMessage;
    private UnitModel _unit;
    private SkillType _type;
    private SkillConfig _skillConfig;
    private SkillData _skill;

    // Use this for initialization
    void Awake()
    {
        _skillConfig = GameConfig.Get<SkillConfig>();
        _abilityMessage = new BuildingMessage( BuildingState.LOCKED, 0 );
    }

    public void SetModel( UnitModel unit, SkillType type )
    {
        _unit = unit;
        _type = type;

        RemoveAllChildren( Container );
        for( int i = 0; i < _skillConfig.Count; i++ )
        {
            _skill = _skillConfig[ i ];
            if( _skill.Type == type )
            {
                GameObject go = Instantiate( SkillUnlockPrefab, Container );
                if( _unit.Skills.ContainsKey( _skill.Index ) )
                    go.GetComponent<SkillUnlockView>().Setup( _unit.Skills[ _skill.Index ], EffectPrefab );
                else
                    go.GetComponent<SkillUnlockView>().Setup( _skill, EffectPrefab );
            }
        }
    }

}
