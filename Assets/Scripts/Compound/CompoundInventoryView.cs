using UniRx;
using UnityEngine.UI;

public class CompoundInventoryView : GameView
{
    public CompoundIconView Icon;
    public Text AmountText;
    public CompoundJSON Compound;

    private CompoundControlMessage _controlMessage = new CompoundControlMessage( 0, CompoundControlAction.REMOVE );

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
        _controlMessage = null;
    }

}
