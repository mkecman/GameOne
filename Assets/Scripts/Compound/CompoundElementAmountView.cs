using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompoundElementAmountView : GameView
{
    public TextMeshProUGUI ElementSymbolAndAmountText;
    public Color RedColor;
    public Color GreenColor;

    public void Setup( LifeElementModel compoundElementModel )
    {
        disposables.Clear();

        if( compoundElementModel == null )
        {
            ElementSymbolAndAmountText.text = "";
        }
        else
        {
            ElementSymbolAndAmountText.text = compoundElementModel.Symbol + compoundElementModel.MaxAmount.ToString();
            compoundElementModel._IsFull.Subscribe( _ => ElementSymbolAndAmountText.color = _ ? GreenColor : RedColor ).AddTo( disposables );
        }
    }
}
