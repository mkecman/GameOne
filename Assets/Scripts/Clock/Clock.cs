using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Clock : MonoBehaviour
{
    private UniRx.IObservable<long> _timer;

    [SerializeField, RangeReactiveProperty( 1, 20 )]
    private IntReactiveProperty _UpdatesPerSecond = new IntReactiveProperty();
    private int _lastUpdatePerSecond; //HACK FOR STUPID INSPECTOR BUG WHICH TRIGGERS VALUE CHANGE ON MOUSE DRAG!

    public ReactiveProperty<long> _ElapsedUpdates = new ReactiveProperty<long>();
    private CompositeDisposable disposable = new CompositeDisposable();

    public void OnEnable()
    {
        GameModel.Register<Clock>( this );
        _UpdatesPerSecond.Where(x => x != _lastUpdatePerSecond ).Subscribe<int>( OnUpdatesPerSecondUpdate );
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
            disposable.Clear();
            _timer = null;
        }
        
        _timer = Observable.Interval( TimeSpan.FromSeconds( 1 / _UpdatesPerSecond.Value ) );
        _timer.Subscribe( x => _ElapsedUpdates.Value++ ).AddTo( disposable );
    }

    
}
