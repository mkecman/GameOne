using System;
using UniRx;
using UnityEngine;

public class Clock : GameView
{
    public bool DebugTime = false;
    public int StepsPerTick = 1;
    public ClockTickMessage message = new ClockTickMessage();

    private IObservable<long> _timer;

    [SerializeField, RangeReactiveProperty( 10, 1000 )]
    private IntReactiveProperty TickEvery_ms = new IntReactiveProperty( 1000 );
    private int _lastTickEvery_ms; //HACK FOR A STUPID INSPECTOR BUG WHICH TRIGGERS VALUE CHANGE ON MOUSE DRAG!
    private DateTime startStopWatch;
    public bool isRunning = false;

    public void OnEnable()
    {
        TickEvery_ms.Where( x => x != _lastTickEvery_ms ).Subscribe( OnTickEveryChange ).AddTo( this );
        GameModel.Set( this );
    }

    public void StartTimer()
    {
        StopTimer();

        _timer = Observable.Interval( TimeSpan.FromMilliseconds( TickEvery_ms.Value ) );
        isRunning = true;

        _timer.Subscribe( x => SendClockMessage() ).AddTo( disposables );
    }

    public void StopTimer()
    {
        disposables.Clear();
        _timer = null;
        isRunning = false;
    }

    private void SendClockMessage()
    {
        message.elapsedTicksSinceStart++;
        message.stepsPerTick = StepsPerTick;

        if( DebugTime )
            startStopWatch = DateTime.Now;

        GameMessage.Send( message );

        if( DebugTime )
            Debug.Log( "UpdateTime: " + ( DateTime.Now - startStopWatch ).ToString() );
    }

    private void OnTickEveryChange( int tickEvery )
    {
        _lastTickEvery_ms = tickEvery;
        if( isRunning )
            StartTimer();
    }
}
