using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CompoundElementAmountView : GameView
{
    public Text Symbol;
    public Text Amount;
    public Color RedColor;
    public Color GreenColor;

    public void Setup( LifeElementModel compoundElementModel )
    {
        disposables.Clear();

        if( compoundElementModel == null )
        {
            Symbol.text = "";
            Amount.text = "";
        }
        else
        {
            Amount.text = compoundElementModel.MaxAmount.ToString();
            Symbol.text = compoundElementModel.Symbol;
            compoundElementModel._IsFull.Subscribe( _ => SetColor( _ ? GreenColor : RedColor ) ).AddTo( disposables );
        }
    }

    private void SetColor( Color color )
    {
        Symbol.color = color;
        Amount.color = color;
    }

}
