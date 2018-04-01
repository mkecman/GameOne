using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class HUDPanelView : GameView
{
    public UIPropertyView Population;
    public UIPropertyView Food;
    public UIPropertyView Science;
    public UIPropertyView Words;

    private LifeModel _life;
    private double _lastEnergy;

    private RDictionary<UIPropertyView> Props = new RDictionary<UIPropertyView>();
    private RDictionary<double> LastProps = new RDictionary<double>();

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );

        Population.SetProperty( R.Population.ToString() );
        Food.SetProperty( R.Energy.ToString() );
        Science.SetProperty( R.Science.ToString() );
        Words.SetProperty( R.Minerals.ToString() );

        Props.Add( R.Energy, Food );
        Props.Add( R.Science, Science );
        Props.Add( R.Minerals, Words );
        LastProps.Add( R.Energy, 0 );
        LastProps.Add( R.Science, 0 );
        LastProps.Add( R.Minerals, 0 );

        GameMessage.Listen<ClockTickMessage>( OnClockTick );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
        disposables.Clear();

        _life.Props[ R.Population ]._Value.Subscribe( _ => Population.SetValue( _ ) ).AddTo( disposables );

        /*
        _life.Props[ R.Energy ]._Value.Subscribe( _ => Food.SetValue( _, _life.Props[ R.Energy ].Delta ) ).AddTo( disposables );
        _life.Props[ R.Science ]._Value.Subscribe( _ => Science.SetValue( _, _life.Props[ R.Science ].Delta ) ).AddTo( disposables );
        _life.Props[ R.Minerals ]._Value.Subscribe( _ => Words.SetValue( _, _life.Props[ R.Minerals ].Delta ) ).AddTo( disposables );

        _life.Props[ R.Energy ]._Delta.Subscribe( _ => Food.SetValue( _life.Props[ R.Energy ].Value, _ ) ).AddTo( disposables );
        _life.Props[ R.Science ]._Delta.Subscribe( _ => Science.SetValue( _life.Props[ R.Science ].Value, _ ) ).AddTo( disposables );
        _life.Props[ R.Minerals ]._Delta.Subscribe( _ => Words.SetValue( _life.Props[ R.Minerals ].Value, _ ) ).AddTo( disposables );
        */
    }

    private void OnClockTick( ClockTickMessage value )
    {
        UpdateProperty( R.Energy );
        UpdateProperty( R.Science );
        UpdateProperty( R.Minerals );
    }

    private void UpdateProperty( R type )
    {
        Props[ type ].SetValue( Math.Round( _life.Props[ type ].Value, 2 ), Math.Round( _life.Props[ type ].Value - LastProps[ type ], 2 ) );
        LastProps[ type ] = _life.Props[ type ].Value;
    }
    
}
