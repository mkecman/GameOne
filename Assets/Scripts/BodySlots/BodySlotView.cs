using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UniRx;

public class BodySlotView : GameView, IDropHandler, IDragHandler
{
    public int Index;
    public CompoundIconView CompoundIcon;
    public Image BackgroundImage;
    public List<Color> StateColors;

    private CompoundInventoryView _dragObject;
    private CompoundConfig _compounds;
    private CompoundEquipMessage _compoundEquipMessage;
    private BodySlotModel _model;

    private void Awake()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _compoundEquipMessage = new CompoundEquipMessage( 0, Index );
    }

    public void OnDrop( PointerEventData eventData )
    {
        if( !_model.IsEnabled )
            return;

        _dragObject = eventData.pointerDrag.GetComponentInParent<CompoundInventoryView>();
        if( _dragObject != null && 
            ( _dragObject.Compound.Type == CompoundType.Armor || _dragObject.Compound.Type == CompoundType.Weapon ) )
        {
            _compoundEquipMessage.CompoundIndex = _dragObject.Compound.Index;
            GameMessage.Send( _compoundEquipMessage );
        }
    }
    public void OnDrag( PointerEventData eventData )
    {
        //needed for OnDrop to work
    }

    internal void Setup( BodySlotModel bodySlotModel )
    {
        disposables.Clear();

        _model = bodySlotModel;
        _model._IsEnabled.Subscribe( _ => OnStateChange() ).AddTo( disposables );
        _model._CompoundIndex.Subscribe( _ => OnCompoundIndexChange( _ ) ).AddTo( disposables );
    }

    private void OnCompoundIndexChange( int _ )
    {
        if( _ != Int32.MaxValue )
        {
            CompoundIcon.gameObject.SetActive( true );
            CompoundIcon.Setup( _compounds[ _ ] );
        }
        else
        {
            CompoundIcon.gameObject.SetActive( false );
        }
    }

    private void OnStateChange()
    {
        BackgroundImage.color = _model.IsEnabled ? StateColors[ 1 ] : StateColors[ 0 ];
    }
}
