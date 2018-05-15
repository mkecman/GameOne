using System;
using TMPro;

public class GameClockView : GameView
{
    public TextMeshProUGUI TextField;

    void Start()
    {
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds( value.elapsedTicksSinceStart );
        TextField.text = string.Format( "{0}d {1:D2}:{2:D2}:{3:D2}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds );
    }

}
