using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UniRx;

public class SkillUnlockPanel : GameView
{
    public Transform Container;
    public GameObject SkillUnlockPrefab;
    public GameObject EffectPrefab;

    private UnitModel _unit;
    private SkillConfig _skillConfig;
    private SkillData _skill;

    // Use this for initialization
    void Awake()
    {
        _skillConfig = GameConfig.Get<SkillConfig>();
    }

    public void SetModel( UnitModel unit, SkillType type )
    {
        _unit = unit;
        
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
