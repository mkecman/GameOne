using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class HUDPanelView : MonoBehaviour
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
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
        _life.Props[ R.Population ]._Value.Subscribe( _ => Population.SetValue( _ ) );
        _life.Props[ R.Energy ]._Value.Subscribe( _ => Food.SetValue( _, _life.Props[ R.Energy ].Delta ) );
        _life.Props[ R.Science ]._Value.Subscribe( _ => Science.SetValue( _, _life.Props[ R.Science ].Delta ) );
        _life.Props[ R.Minerals ]._Value.Subscribe( _ => Words.SetValue( _, _life.Props[ R.Minerals ].Delta ) );
    }
    
}
