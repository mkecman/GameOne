using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AbilityController : AbstractController
{
    private UnitModel _unit;
    private AbilityData _selectedAbility;
    private List<AbilityData> _abilitiesConfig;
    private AbilityPaymentService _pay;

    public AbilityController()
    {
        if( _pay == null )
            _pay = GameModel.Get<AbilityPaymentService>();

        GameMessage.Listen<AbilityMessage>( OnAbilityMessage );
        
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        _abilitiesConfig = Config.Get<AbilityConfig>().Abilities;
    }

    private void OnAbilityMessage( AbilityMessage value )
    {
        switch( value.State )
        {
            case AbilityState.LOCKED:
                break;
            case AbilityState.UNLOCKED:
                UnlockAbility( value.Index );
                break;
            case AbilityState.ACTIVE:
                ActivateAbility( value.Index );
                break;
            case AbilityState.SELECTED:
                SelectAbility( value.Index );
                break;
            default:
                break;
        }
        GameModel.Set( _unit );
    }

    private void SelectAbility( int index )
    {
        _selectedAbility = _abilitiesConfig[ index ];
        GameModel.Set( _selectedAbility );
    }
    
    private void ActivateAbility( int index )
    {
        if( !_unit.Abilities.ContainsKey( index ) )
            _unit.Abilities.Add( index, AbilityState.ACTIVE );
        else
            _unit.Abilities[ index ] = AbilityState.ACTIVE;

        foreach( KeyValuePair<R, double> item in _abilitiesConfig[ index ].Effects )
        {
            _unit.AbilitiesDelta[ item.Key ].Value += item.Value;
        }
    }

    private void UnlockAbility( int index )
    {
        //If already active, return to the unlocked state
        if( _unit.Abilities.ContainsKey( index ) && _unit.Abilities[ index ] == AbilityState.ACTIVE )
        {
            foreach( KeyValuePair<R, double> item in _abilitiesConfig[ index ].Effects )
            {
                _unit.AbilitiesDelta[ item.Key ].Value -= item.Value;
            }
            _unit.Abilities[ index ] = AbilityState.UNLOCKED;
        }
        
        //if this ability hasnt been unlocked and the player can unlock it
        if( !_unit.Abilities.ContainsKey( index ) && _pay.BuyUnlockAbility( index ) )
        {
            _unit.Abilities.Add( index, AbilityState.UNLOCKED );
        }
    }

    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
    }
    
}
