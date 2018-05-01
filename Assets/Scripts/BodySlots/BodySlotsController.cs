using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class BodySlotsController : IGameInit
{
    private CompositeDisposable disposables = new CompositeDisposable();
    private UnitModel _unit;

    public void Init()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange ); 
    }

    private void OnUnitChange( UnitModel value )
    {
        disposables.Clear();
        _unit = value;

        foreach( KeyValuePair<int, BodySlotModel> item in _unit.BodySlots )
        {
            item.Value._CompoundIndex.Subscribe( _ => SetupBodySlot( item.Value ) ).AddTo( disposables );
        }
    }

    private void SetupBodySlot( BodySlotModel value )
    {
        
    }
}
