using UnityEngine;
using System.Collections;
using System;

public class UnitPassiveSkillsPanel : GameView
{
    public GameObject ButtonPrefab;
    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnUnitChange( UnitModel value )
    {
        RemoveAllChildren( transform );
        if( value != null )
        for( int i = 0; i < value.PassiveSkills.Count; i++ )
        {
            Add( value.PassiveSkills[ i ] );
        }
    }

    private void Add( SkillType type )
    {
        GameObject go = Instantiate( ButtonPrefab, transform );
        go.GetComponent<PassiveSkillButton>().Type = type;
    }
}
