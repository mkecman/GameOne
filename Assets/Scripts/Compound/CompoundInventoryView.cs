using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using System;

public class CompoundInventoryView : GameView, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public CompoundIconView Icon;
    public Text AmountText;
    public CompoundJSON Compound;

    private GameObject Copy;
    private Transform DragPanel;
    private CompoundControlMessage _controlMessage;
    

    private void Awake()
    {
        DragPanel = GameObject.Find( "DragPanel" ).transform;
        
        _controlMessage = new CompoundControlMessage();
        _controlMessage.Action = CompoundControlAction.REMOVE;
    }

    internal void Setup( CompoundJSON compound, IntReactiveProperty amount )
    {
        disposables.Clear();
        Compound = compound;
        _controlMessage.Index = compound.Index;
        amount.Subscribe( _ => OnAmountChange( _ ) ).AddTo( disposables );
        Icon.Setup( compound );
    }

    private void OnAmountChange( int value )
    {
        AmountText.text = value.ToString();

        if( value <= 0 )
        {
            GameMessage.Send( _controlMessage );
            Destroy( gameObject );
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Copy = null;
        DragPanel = null;
        Compound = null;
    }

    public void OnDrag( PointerEventData eventData )
    {
        Copy.transform.position = eventData.position;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        Destroy( Copy );
        Copy = null;
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        Copy = Instantiate( this.gameObject, DragPanel );
        Copy.GetComponent<CompoundInventoryView>().Icon.IsRaycastTarget = false;
    }
}
