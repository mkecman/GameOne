using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Clock : MonoBehaviour
{
    private UniRx.IObservable<long> _timer;

    [SerializeField, RangeReactiveProperty( 1, 20 )]
    private IntReactiveProperty _UpdatesPerSecond = new IntReactiveProperty();
    private int _lastUpdatePerSecond; //HACK FOR A STUPID INSPECTOR BUG WHICH TRIGGERS VALUE CHANGE ON MOUSE DRAG!

    public ReactiveProperty<long> ElapsedUpdates = new ReactiveProperty<long>();
    private CompositeDisposable disposables = new CompositeDisposable();

    public void OnEnable()
    {
        GameModel.Register<Clock>( this );
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
        
        _timer = Observable.Interval( TimeSpan.FromSeconds( 1 / _UpdatesPerSecond.Value ) );
        _timer.Subscribe( x => ElapsedUpdates.Value++ ).AddTo( disposables );
    }

    
}
