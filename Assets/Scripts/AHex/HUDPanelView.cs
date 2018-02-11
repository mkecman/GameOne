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
        _life._Population.Subscribe( _ => Population.SetValue( _.ToString() ) );
        _life._Food.Subscribe( _ => Food.SetValue( _.ToString() ) );
        _life._Science.Subscribe( _ => Science.SetValue( _.ToString() ) );
        _life._Words.Subscribe( _ => Words.SetValue( _.ToString() ) );
    }

    // Update is called once per frame
    void Update()
    {

    }
}
