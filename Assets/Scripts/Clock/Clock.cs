using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Clock : MonoBehaviour
{
    private UniRx.IObservable<long> _timer;

    [SerializeField, RangeReactiveProperty( 100, 1000 )]
    private IntReactiveProperty _UpdatesPerSecond = new IntReactiveProperty(1000);
    private int _lastUpdatePerSecond; //HACK FOR A STUPID INSPECTOR BUG WHICH TRIGGERS VALUE CHANGE ON MOUSE DRAG!

    public ReactiveProperty<long> ElapsedUpdates = new ReactiveProperty<long>();
    private CompositeDisposable disposables = new CompositeDisposable();

    public void OnEnable()
    {
        GameModel.Register<Clock>( this );
        _UpdatesPerSecond.Value = 1000;
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
