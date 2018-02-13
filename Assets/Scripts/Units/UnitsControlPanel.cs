using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class UnitsControlPanel : GameView
{
    public Button AddUnitButton;
    public Button EvolveButton;
    public GameObject EvolvePanel;

    private PlanetModel _planet;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        AddUnitButton.OnClickAsObservable().Subscribe( _ => { GameMessage.Send<UnitMessage>( new UnitMessage( UnitMessageType.Add, 10, 10 ) ); } ).AddTo( disposables );
        EvolveButton.OnClickAsObservable().Subscribe( _ => SetEvolvePanel() ).AddTo( disposables );
    }

    private void SetEvolvePanel()
    {
        if( EvolvePanel.activeSelf )
            EvolvePanel.SetActive( false );
        else
            EvolvePanel.SetActive( true );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _planet = value;
        _planet.Life._Population.Subscribe( population => AddUnitButton.GetComponentInChildren<Text>().text = "Add: " + (int)( _planet.Life.Population * 4 ) * 10 ).AddTo( disposables );
    }
    
}
