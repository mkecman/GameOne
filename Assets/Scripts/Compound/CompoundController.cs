using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class CompoundController : IGameInit
{
    private LifeModel _life;
    private CompoundConfig _compounds;
    private UnitEquipCommand _unitEquipCommand;
    private CompoundPaymentService _pay;

    public void Init()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _unitEquipCommand = GameModel.Get<UnitEquipCommand>();
        _pay = GameModel.Get<CompoundPaymentService>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<CompoundControlMessage>( OnCompoundControlMessage );
        GameMessage.Listen<CompoundDropMessage>( OnCompoundDropMessage );
    }

    private void OnCompoundDropMessage( CompoundDropMessage value )
    {
        _unitEquipCommand.Execute( value.CompoundIndex, value.BodySlotIndex );
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
                AddCompound( value.Index );
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

    private void AddCompound( int index )
    {
        if( _pay.BuyCompound( index ) )
            if( _life.Compounds.ContainsKey( index ) )
                _life.Compounds[ index ].Value++;
            else
                _life.Compounds.Add( index, new IntReactiveProperty( 1 ) );
    }
}
