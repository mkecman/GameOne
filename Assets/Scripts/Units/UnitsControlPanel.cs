using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class UnitsControlPanel : GameView
{
    public Button CloneButton;
    public Button MoveButton;

    private PlanetModel _planet;

    private UnitPaymentService _unitPaymentService;

    // Use this for initialization
    void Start()
    {
        _unitPaymentService = GameModel.Get<UnitPaymentService>();
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _planet = value;
        _planet.Life.Props[ R.Population ]._Value.Subscribe( population => CloneButton.GetComponentInChildren<Text>().text = "Add: " + _unitPaymentService.GetAddUnitPrice() ).AddTo( disposables );
    }
    
}
