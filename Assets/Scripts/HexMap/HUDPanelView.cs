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

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );

        Population.SetProperty( R.Population.ToString() );
        Food.SetProperty( R.Energy.ToString() );
        Science.SetProperty( R.Science.ToString() );
        Words.SetProperty( R.Minerals.ToString() );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
        disposables.Clear();

        _life.Props[ R.Population ]._Value.Subscribe( _ => Population.SetValue( _ ) ).AddTo( disposables );

        _life.Props[ R.Energy ]._Value.Subscribe( _ => Food.SetValue( _, _life.Props[ R.Energy ].Delta ) ).AddTo( disposables );
        _life.Props[ R.Science ]._Value.Subscribe( _ => Science.SetValue( _, _life.Props[ R.Science ].Delta ) ).AddTo( disposables );
        _life.Props[ R.Minerals ]._Value.Subscribe( _ => Words.SetValue( _, _life.Props[ R.Minerals ].Delta ) ).AddTo( disposables );

        _life.Props[ R.Energy ]._Delta.Subscribe( _ => Food.SetValue( _life.Props[ R.Energy ].Value, _ ) ).AddTo( disposables );
        _life.Props[ R.Science ]._Delta.Subscribe( _ => Science.SetValue( _life.Props[ R.Science ].Value, _ ) ).AddTo( disposables );
        _life.Props[ R.Minerals ]._Delta.Subscribe( _ => Words.SetValue( _life.Props[ R.Minerals ].Value, _ ) ).AddTo( disposables );
    }
    
}
