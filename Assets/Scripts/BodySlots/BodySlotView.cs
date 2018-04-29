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
    public Image BackgroundImage;
    public List<Color> StateColors;

    private CompoundInventoryView _dragObject;
    private CompoundDropMessage _compoundDropMessage;
    private BodySlotModel _model;

    private void Start()
    {
        _compoundDropMessage = new CompoundDropMessage( 0, Index );
    }

    public void OnDrag( PointerEventData eventData )
    {
        //needed for OnDrop to work
    }

    public void OnDrop( PointerEventData eventData )
    {
        Debug.Log( "OnDrop" );
        if( !_model.IsEnabled )
            return;

        _dragObject = eventData.pointerDrag.GetComponent<CompoundInventoryView>();
        if( _dragObject != null )
        {
            Debug.Log( "DROPPPPPERD" );
            _compoundDropMessage.CompoundIndex = _dragObject.Compound.Index;
            GameMessage.Send( _compoundDropMessage );
            //_dragObject.Copy.transform.SetParent( transform );
        }
    }

    internal void Setup( BodySlotModel bodySlotModel )
    {
        disposables.Clear();

        _model = bodySlotModel;
        _model._IsEnabled.Subscribe( _ => OnStateChange() ).AddTo( disposables );
        _model._CompoundIndex.Subscribe( _ => Debug.Log( _.ToString() ) ).AddTo( disposables );
    }

    private void OnStateChange()
    {
        BackgroundImage.color = _model.IsEnabled ? StateColors[ 1 ] : StateColors[ 0 ];
    }
}
