using System;
using UniRx;
using UnityEngine.UI;

public class UnitPropUpgradeView : GameView
{
    public UIPropertyView PropertyView;
    public Button UpgradeButton;
    private int delta = 0;

    public void SetModel( R prop, UnitModel unit, bool canChange )
    {
        disposables.Clear();
        delta = 0;

        PropertyView.SetProperty( prop.ToString() );

        unit.Props[ prop ]._Value
            .Subscribe( _ => PropertyView.SetValue( _ ) )
            .AddTo( disposables );

        unit.Props[ R.UpgradePoint ]._Value
            .Subscribe( _ => UpgradeButton.interactable = _ > 0 ? true : false )
            .AddTo( disposables );

        if( canChange )
        {
            UpgradeButton.OnClickAsObservable()
                .Subscribe( _ =>
                {
                    GameMessage.Send( new UnitPropUpgradeMessage( prop ) );
                    PropertyView.SetDelta( ++delta );
                } )
                .AddTo( disposables );
        }
        else
        {
            UpgradeButton.gameObject.SetActive( false );
        }
    }
}
