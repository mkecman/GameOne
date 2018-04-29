using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;

public class BodySlotsView : GameView
{
    public Transform Container;
    public GameObject BodySlotPrefab;
    
    private UnitModel _unit;
    private BodySlotsConfig _slotsConfig;
    private List<int> _slots;

    // Use this for initialization
    void Start()
    {
        _slotsConfig = GameConfig.Get<BodySlotsConfig>();
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnUnitChange( UnitModel value )
    {
        disposables.Clear();
        _unit = value;
        if( _unit == null )
            return;

        _unit.Props[ R.Body ]._Value.Subscribe( _ => OnUnitBodyChange() ).AddTo( disposables );
    }

    private void OnUnitBodyChange()
    {
        _slots = _slotsConfig[ (int)( _unit.Props[ R.Body ].Value / 8.34f ) ];

        for( int i = 0; i < _slots.Count; i++ )
        {
            if( _slots[ i ] == 1 )
            {
                Container.GetChild( i ).GetComponent<BodySlotView>().IsEnabled = true;
            }
            else
            {
                Container.GetChild( i ).GetComponent<BodySlotView>().IsEnabled = false;
            }
        }
    }
}
