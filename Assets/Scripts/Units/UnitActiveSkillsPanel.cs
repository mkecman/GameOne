using UnityEngine;
using System.Collections;
using System;

public class UnitActiveSkillsPanel : GameView
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
        for( int i = 0; i < value.ActiveSkills.Count; i++ )
        {
            Add( value.ActiveSkills[ i ] );
        }
    }

    private void Add( int index )
    {
        GameObject go = Instantiate( ButtonPrefab, transform );
        go.GetComponent<ActiveSkillButton>().Index = index;
    }
}
