using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Clock : MonoBehaviour
{
    public ReactiveProperty<long> ElapsedUpdates = new ReactiveProperty<long>();
    public ClockTickMessage message = new ClockTickMessage();


    private IObservable<long> _timer;

    [SerializeField, RangeReactiveProperty( 10, 1000 )]
    private IntReactiveProperty _UpdatesPerSecond = new IntReactiveProperty(1000);
    private int _lastUpdatePerSecond; //HACK FOR A STUPID INSPECTOR BUG WHICH TRIGGERS VALUE CHANGE ON MOUSE DRAG!

    private CompositeDisposable disposables = new CompositeDisposable();

    public void OnEnable()
    {
        GameModel.Set<Clock>( this );
        //_UpdatesPerSecond.Value = 1000;
        _UpdatesPerSecond.Where(x => x != _lastUpdatePerSecond ).Subscribe<int>( OnUpdatesPerSecondUpdate ).AddTo( this );
    }

    private void OnUpdatesPerSecondUpdate( int UPS )
    {
        _lastUpdatePerSecond = UPS;
        StartTimer();
    }

    private void StartTimer()
    {
        if( _timer != null )
        {
            disposables.Clear();
            _timer = null;
        }
        
        _timer = Observable.Interval( TimeSpan.FromMilliseconds( _UpdatesPerSecond.Value ) );
        _timer.Subscribe( x => ElapsedUpdates.Value++ ).AddTo( disposables );
    }

    
}
