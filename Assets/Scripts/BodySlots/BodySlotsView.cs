using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;

public class BodySlotsView : GameView
{
    public Transform Container;
    public GameObject BodySlotPrefab;
    
    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnUnitChange( UnitModel unit )
    {
        if( unit == null )
            return;

        for( int i = 0; i < unit.BodySlots.Count; i++ )
        {
            Container.GetChild( i ).GetComponent<BodySlotView>().Setup( unit.BodySlots[ i ] );
        }
    }

}
