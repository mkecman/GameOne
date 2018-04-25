﻿using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CompoundElementAmountView : GameView
{
    public Text Symbol;
    public Text Amount;
    public Color RedColor;
    public Color GreenColor;
    private int _requiredAmount;
    private BoolReactiveProperty _canCraft;

    public void Setup( LifeElementModel lifeElementModel, int requiredAmount, BoolReactiveProperty canCraft )
    {
        _canCraft = canCraft;
        disposables.Clear();
        if( lifeElementModel == null )
        {
            Symbol.text = "";
            Amount.text = "";
        }
        else
        {
            _requiredAmount = requiredAmount;
            Amount.text = _requiredAmount.ToString();
            Symbol.text = lifeElementModel.Symbol;

            lifeElementModel._Amount.Subscribe( _ => SetColor( _ ) ).AddTo( disposables );
        }
    }

    private void SetColor( int amount )
    {
        if( amount < _requiredAmount )
        {
            Symbol.color = RedColor;
            Amount.color = RedColor;
            _canCraft.Value = false;
        }
        else
        {
            Symbol.color = GreenColor;
            Amount.color = GreenColor;
            _canCraft.Value = _canCraft.Value && true;
        }
    }
}
