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
    private CompoundEquipMessage _compoundDropMessage;
    private BodySlotModel _model;

    private void Awake()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        _compoundDropMessage = new CompoundEquipMessage( 0, Index );

    }

    public void OnDrop( PointerEventData eventData )
    {
        //Debug.Log( "OnDrop" );
        if( !_model.IsEnabled )
            return;

        _dragObject = eventData.pointerDrag.GetComponentInParent<CompoundInventoryView>();
        if( _dragObject != null )
        {
            //Debug.Log( "DROPPPPPERD" );
            _compoundDropMessage.CompoundIndex = _dragObject.Compound.Index;
            GameMessage.Send( _compoundDropMessage );
            //_dragObject.Copy.transform.SetParent( transform );
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
