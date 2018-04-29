using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using System;

public class CompoundInventoryView : GameView, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RawImage CompoundTexture;
    public Text AmountText;
    public CompoundJSON Compound;
    public GameObject Copy;

    private Transform DragPanel;
    private int _amount;

    private void Awake()
    {
        DragPanel = GameObject.Find( "DragPanel" ).transform;
    }

    internal void Setup( CompoundJSON compound, IntReactiveProperty amount )
    {
        disposables.Clear();
        Compound = compound;
        amount.Subscribe( _ => OnAmountChange( _ ) ).AddTo( disposables );
        CompoundTexture.texture = compound.Texture;
    }

    private void OnAmountChange( int value )
    {
        _amount = value;
        AmountText.text = value.ToString();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Copy = null;
        DragPanel = null;
        Compound = null;
        CompoundTexture.texture = null;
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
        Copy.GetComponent<CompoundInventoryView>().CompoundTexture.raycastTarget = false;
    }
}
