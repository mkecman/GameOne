using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class UnitsControlPanel : GameView
{
    public Button AddUnitButton;

    private PlanetModel _planet;

    private UnitPaymentService _unitPaymentService;

    // Use this for initialization
    void Start()
    {
        _unitPaymentService = GameModel.Get<UnitPaymentService>();
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        AddUnitButton.OnClickAsObservable().Subscribe( _ => 
        {
            GameMessage.Send<UnitMessage>( new UnitMessage( UnitMessageType.Add, 10, 10 ) ); } ).AddTo( disposables );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _planet = value;
        _planet.Life.Props[ R.Population ]._Value.Subscribe( population => AddUnitButton.GetComponentInChildren<Text>().text = "Add: " + _unitPaymentService.GetAddUnitPrice() ).AddTo( disposables );
    }
    
}
