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
        _life._Population.Subscribe( _ => Population.SetValue( _ ) );
        _life._Food.Subscribe( _ => Food.SetValue( _, _life.FoodDelta ) );
        _life._Science.Subscribe( _ => Science.SetValue( _, _life.ScienceDelta ) );
        _life._Words.Subscribe( _ => Words.SetValue( _, _life.WordsDelta ) );
    }
    
}
