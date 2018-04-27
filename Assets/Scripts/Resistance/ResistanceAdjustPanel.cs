using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class ResistanceAdjustPanel : GameView
{
    public ResistanceGraph ResistanceGraph;
    public Button IncreaseButton;
    public Button DecreaseButton;
    public Text ScienceCostText;

    private UnitModel _selectedUnit;
    private ResistanceUpgradeMessage _resistanceMessage = new ResistanceUpgradeMessage();

    private void Start()
    {
        _resistanceMessage = new ResistanceUpgradeMessage();
        _resistanceMessage.Type = ResistanceGraph.Lens;
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnDecreaseButtonClick()
    {
        _resistanceMessage.Delta = -.01f;
        GameMessage.Send( _resistanceMessage );
    }

    private void OnIncreaseButtonClick()
    {
        _resistanceMessage.Delta = .01f;
        GameMessage.Send( _resistanceMessage );
    }

    public void OnUnitChange( UnitModel value )
    {
        disposables.Clear();
        _selectedUnit = value;
        if( _selectedUnit != null )
        {
            _selectedUnit.Resistance[ ResistanceGraph.Lens ].Consumption.Subscribe( _ => ScienceCostText.text = _.ToString() ).AddTo( disposables );
            IncreaseButton.OnClickAsObservable().Subscribe( _ => OnIncreaseButtonClick() ).AddTo( disposables );
            DecreaseButton.OnClickAsObservable().Subscribe( _ => OnDecreaseButtonClick() ).AddTo( disposables );
        }
    }
    
}
