﻿using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class CompoundController : IGameInit
{
    private LifeModel _life;
    private UnitEquipCommand _unitEquipCommand;
    private CompoundPaymentService _pay;

    public void Init()
    {
        _unitEquipCommand = GameModel.Get<UnitEquipCommand>();
        _pay = GameModel.Get<CompoundPaymentService>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<CompoundControlMessage>( OnCompoundControlMessage );
        GameMessage.Listen<CompoundEquipMessage>( OnCompoundDropMessage );
    }

    private void OnCompoundDropMessage( CompoundEquipMessage value )
    {
        if( value.Action == CompoundEquipAction.EQUIP )
            _unitEquipCommand.ExecuteEquip( value.CompoundIndex, value.BodySlotIndex );
        else
            _unitEquipCommand.ExecuteUnequip( value.BodySlotIndex );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
    }
    
    private void OnCompoundControlMessage( CompoundControlMessage value )
    {
        switch( value.Action )
        {
            case CompoundControlAction.ADD:
                AddCompound( value.Index, value.SpendCurrency );
                break;
            case CompoundControlAction.REMOVE:
                RemoveCompound( value.Index );
                break;
            default:
                break;
        }
    }

    private void RemoveCompound( int index )
    {
        if( _life.Compounds.ContainsKey( index ) )
        {
            _life.Compounds[ index ].Dispose();
            _life.Compounds.Remove( index );
        }
    }

    private void AddCompound( int index, bool spendCurrency )
    {
        if( _pay.BuyCompound( index, spendCurrency ) )
            if( _life.Compounds.ContainsKey( index ) )
                _life.Compounds[ index ].Value++;
            else
                _life.Compounds.Add( index, new IntReactiveProperty( 1 ) );
    }
}
