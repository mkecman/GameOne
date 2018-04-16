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

    private void Awake()
    {
        
    }

    void OnEnable()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        OkButton.onClick.AddListener( OnOkButtonClick );
    }

    void OnDisable()
    {
        GameModel.RemoveHandle<UnitModel>( OnUnitChange );
        OkButton.onClick.RemoveListener( OnOkButtonClick );
        disposables.Clear();
    }

    private void OnOkButtonClick()
    {
        gameObject.SetActive( false );
    }

    private void OnUnitChange( UnitModel value )
    {
        disposables.Clear();
        _selectedUnit = value;

        if( _selectedUnit != null )
        {
            Temperature.SetUnit( _selectedUnit );
            Humidity.SetUnit( _selectedUnit );
            Pressure.SetUnit( _selectedUnit );
            Radiation.SetUnit( _selectedUnit );
            
            //_selectedUnit.AbilitiesDelta[ R.Science ].Subscribe( _ => ScienceCost.text = _.ToString() ).AddTo( disposables );
        }
    }
    
}
