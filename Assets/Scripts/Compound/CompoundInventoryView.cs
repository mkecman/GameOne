using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;
using System;

public class CompoundInventoryView : GameView
{
    public CompoundIconView Icon;
    public Text AmountText;
    public CompoundJSON Compound;

    private CompoundControlMessage _controlMessage;
    

    private void Awake()
    {
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
        Compound = null;
    }

}
