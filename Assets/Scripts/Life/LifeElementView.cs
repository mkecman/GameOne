using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

public class LifeElementView : GameView
{
    public ProgressBar Bar;
    public Text SymbolText;
    public Text AmountText;
    private LifeElementModel _model;

    public void SetModel( LifeElementModel lifeElementModel )
    {
        disposables.Clear();
        _model = lifeElementModel;

        SymbolText.text = _model.Symbol;
        Bar.MaxValue = _model.MaxAmount;
        _model._Amount.Subscribe( _ => 
        {
            Bar.Value = _;
            AmountText.text = _.ToString();
            gameObject.SetActive( _ > 0 );
        } ).AddTo( disposables );
    }
}
