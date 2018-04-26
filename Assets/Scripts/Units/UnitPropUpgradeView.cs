using UniRx;
using UnityEngine.UI;

public class UnitPropUpgradeView : GameView
{
    public UIPropertyView PropertyView;
    public Button UpgradeButton;

    public void SetModel( R prop, UnitModel unit )
    {
        disposables.Clear();

        PropertyView.SetProperty( prop.ToString() );

        unit.Props[ prop ]._Value
            .Subscribe( _ => PropertyView.SetValue( _ ) )
            .AddTo( disposables );

        unit.Props[ R.UpgradePoint ]._Value
            .Subscribe( _ => UpgradeButton.interactable = _ > 0 ? true : false )
            .AddTo( disposables );

        UpgradeButton.OnClickAsObservable()
            .Subscribe( _ => GameMessage.Send( new UnitPropUpgradeMessage( prop ) ) )
            .AddTo( disposables );
    }

}
