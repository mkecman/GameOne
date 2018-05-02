using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CompoundInventoryPanel : GameView, IDropHandler, IDragHandler
{
    public GameObject CompoundInventoryPrefab;
    private ReactiveDictionary<int, IntReactiveProperty> _LifeCompounds;
    private CompoundConfig _compounds;
    private CompoundEquipMessage _compoundEquipMessage;
    private BodySlotView _dragObject;

    private void Start()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        _compoundEquipMessage = new CompoundEquipMessage( 0, 0, CompoundEquipAction.UNEQUIP );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        disposables.Clear();
        RemoveAllChildren( transform );

        _LifeCompounds = value.Life.Compounds;

        foreach( KeyValuePair<int, IntReactiveProperty> item in _LifeCompounds )
        {
            Add( _compounds[ item.Key ], item.Value );
        }

        _LifeCompounds.ObserveAdd().Subscribe( _ => Add( _compounds[ _.Key ], _.Value ) ).AddTo( disposables );
        //_LifeCompounds.ObserveRemove().Subscribe( _ => Remove( _.Key ) ).AddTo( disposables );
    }

    private void Add( CompoundJSON compoundJSON, IntReactiveProperty value )
    {
        Instantiate( CompoundInventoryPrefab, transform )
            .GetComponent<CompoundInventoryView>()
            .Setup( compoundJSON, value );
    }

    public void OnDrop( PointerEventData eventData )
    {
        Debug.Log( "OnDrop" );

        _dragObject = eventData.pointerDrag.GetComponentInParent<BodySlotView>();
        if( _dragObject != null )
        {
            Debug.Log( "DROPPPPPERD" );
            _compoundEquipMessage.BodySlotIndex = _dragObject.Index;
            GameMessage.Send( _compoundEquipMessage );
        }
    }

    public void OnDrag( PointerEventData eventData )
    {
        //needed for OnDrop to work
    }
}
