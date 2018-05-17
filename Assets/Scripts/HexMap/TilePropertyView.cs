using UniRx;

public class TilePropertyView : GameView
{
    public R ShowProperty;
    public UIPropertyView PropertyView;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<HexModel>( OnHexModel );
    }

    private void OnHexModel( HexModel value )
    {
        disposables.Clear();

        value.Props[ ShowProperty ]._Value.Subscribe( _ => PropertyView.SetValue( _ ) ).AddTo( disposables );
    }

}
