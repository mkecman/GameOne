using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class UnitResistancePanel : GameView
{
    public ResistanceAdjustPanel Temperature;
    public ResistanceAdjustPanel Humidity;
    public ResistanceAdjustPanel Pressure;
    public ResistanceAdjustPanel Radiation;
    public Text ScienceCost;
    public Button OkButton;

    private UnitModel _selectedUnit;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        OkButton.onClick.AddListener( OnOkButtonClick );
    }

    private void OnOkButtonClick()
    {
        gameObject.SetActive( false );
    }

    private void OnUnitChange( UnitModel value )
    {
        _selectedUnit = value;
        Temperature.SetUnit( _selectedUnit );
        Humidity.SetUnit( _selectedUnit );
        Pressure.SetUnit( _selectedUnit );
        Radiation.SetUnit( _selectedUnit );

        disposables.Clear();
        _selectedUnit.AbilitiesDelta[ R.Science ].Subscribe( _ => ScienceCost.text = _.ToString() ).AddTo( disposables );
    }
    
}
