using System;
using TMPro;
using UniRx;

public class GameClockView : GameView
{
    public TextMeshProUGUI TextField;
    private TimeSpan _timeSpan;

    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        value._Time.Subscribe( OnClockTick ).AddTo( disposables );
    }

    private void OnClockTick( long value )
    {
        _timeSpan = TimeSpan.FromSeconds( value );
        TextField.text = string.Format( "{0}d {1:D2}h:{2:D2}m:{3:D2}s", _timeSpan.Days, _timeSpan.Hours, _timeSpan.Minutes, _timeSpan.Seconds );
    }

}
