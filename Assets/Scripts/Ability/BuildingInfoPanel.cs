using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;
using System.Collections.Generic;

public class BuildingInfoPanel : GameView
{
    public Text NameText;
    public Text DescriptionText;
    public Button DemolishButton;
    public Button ActivateButton;
    public Button BuildButton;
    public Transform MaintenanceGrid;
    public Transform EffectsGrid;
    public GameObject BuildPanel;
    public GameObject EffectPrefab;

    public Color Activated;
    public Color Deactivated;
    public Image BackgroundImage;
    
    private HexModel _hex;
    private BuildingModel _building;

    private Text _activateButtonText;
    private BuildingMessage _abilityMessage;
    private BuildingPaymentService _pay;

    // Use this for initialization
    void Start()
    {
        _pay = GameModel.Get<BuildingPaymentService>();
        _abilityMessage = new BuildingMessage( BuildingState.UNLOCKED, 0 );

        _activateButtonText = ActivateButton.GetComponentInChildren<Text>();
        GameModel.HandleGet<HexModel>( OnHexChange );

        DemolishButton.OnClickAsObservable().Subscribe( _ => OnDemolishButtonClick() );
        ActivateButton.OnClickAsObservable().Subscribe( _ => OnActivateButtonClick() );
    }

    private void OnHexChange( HexModel value )
    {
        _hex = value;
        if( _hex == null )
        {
            _building = null;
            BuildPanel.SetActive( true );
            BuildButton.interactable = false;
        }
        else
            if( _hex.Building != null )
            {
                SetBuilding( _hex );
                BuildPanel.SetActive( false );
            }
            else
            {
                _building = null;
                BuildPanel.SetActive( true );
                if( _hex.Unit == null )
                    BuildButton.interactable = false;
                else
                    BuildButton.interactable = true;
            }
    }

    private void OnDemolishButtonClick()
    {
        _abilityMessage.State = BuildingState.DEMOLISH;
        GameMessage.Send( _abilityMessage );
    }

    private void OnActivateButtonClick()
    {
        if( _building.State == BuildingState.INACTIVE )
            _abilityMessage.State = BuildingState.ACTIVE;
        else
            _abilityMessage.State = BuildingState.INACTIVE;

        GameMessage.Send( _abilityMessage );
    }
    
    private void SetBuilding( HexModel hex )
    {
        disposables.Clear();

        _building = _hex.Building;
        _building._State.Subscribe( _ => SetState() ).AddTo( disposables );

        _abilityMessage.Index = _building.Index;
        _abilityMessage.X = _building.X;
        _abilityMessage.Y = _building.Y;

        NameText.text = _building.Name;
        DescriptionText.text = "PRICE: " + _pay.GetUnlockPrice( _building.Index );

        RemoveAllChildren( EffectsGrid );
        RemoveAllChildren( MaintenanceGrid );

        foreach( KeyValuePair<string, double> item in _building.Effects )
        {
            if( item.Key == R.Minerals.ToString() )
                AddEffect( MaintenanceGrid, R.Minerals.ToString(), item.Value );
            else
            if( item.Value < 0 )
                AddEffect( EffectsGrid, item.Key, item.Value );
        }
        foreach( KeyValuePair<string, double> item in _building.Effects )
        {
            if( item.Value > 0 )
                AddEffect( EffectsGrid, item.Key, item.Value );
        }

        if( hex.Unit != null )
            SetState();
        else
        {
            ActivateButton.interactable = false;
            DemolishButton.interactable = false;
        }
    }

    private void SetState()
    {
        switch( _building.State )
        {
            case BuildingState.ACTIVE:
                DemolishButton.interactable = true;
                ActivateButton.interactable = true;
                _activateButtonText.text = "Deactivate";
                _activateButtonText.color = Deactivated;
                BackgroundImage.color = Activated;
                break;
            case BuildingState.INACTIVE:
                DemolishButton.interactable = true;
                ActivateButton.interactable = true;
                _activateButtonText.text = "Activate";
                _activateButtonText.color = Activated;
                BackgroundImage.color = Deactivated;
                break;
        }
    }

    private void AddEffect( Transform container, string type, double value )
    {
        GameObject go = Instantiate( EffectPrefab, container );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.SetProperty( type );
        uipv.SetValue( double.MaxValue, value );
    }

    private void RemoveAllChildren( Transform transform )
    {
        GameObject go;
        while( transform.childCount != 0 )
        {
            go = transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }
    }

}
