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

    private BuildingMessage _abilityMessage;
    private UnitModel _unit;
    private SkillType _type;
    private List<SkillData> _skills;
    private SkillConfig _skillConfig;

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
        _skills = _skillConfig[ type ];

        RemoveAllChildren( Container );
        for( int i = 0; i < _skills.Count; i++ )
        {
            GameObject go = Instantiate( SkillUnlockPrefab, Container );
            go.GetComponent<SkillUnlockView>().Setup( _skills[ i ], EffectPrefab );
        }
    }

}
