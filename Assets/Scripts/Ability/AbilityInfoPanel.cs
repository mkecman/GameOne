using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;
using System.Collections.Generic;

public class AbilityInfoPanel : GameView
{
    public Text NameText;
    public Text DescriptionText;
    public Button UnlockButton;
    public Button ActivateButton;
    public Transform IncreaseEffectsGrid;
    public Transform DecreaseEffectsGrid;
    public GameObject EffectPrefab;
    private UnitModel _selectedUnit;

    private Text _activateButtonText;
    private AbilityData _ability;
    private AbilityMessage _abilityMessage;
    private AbilityPaymentService _abilityPayment;

    // Use this for initialization
    void Start()
    {
        _abilityPayment = GameModel.Get<AbilityPaymentService>();
        _abilityMessage = new AbilityMessage( AbilityState.UNLOCKED, 0 );

        _activateButtonText = ActivateButton.GetComponentInChildren<Text>();
        GameModel.HandleGet<AbilityData>( OnAbilityChange );
        GameModel.HandleGet<UnitModel>( OnSelectedUnitChange );

        UnlockButton.OnClickAsObservable().Subscribe( _ => OnUnlockButtonClick() );
        ActivateButton.OnClickAsObservable().Subscribe( _ => OnActivateButtonClick() );
    }

    private void OnUnlockButtonClick()
    {
        _abilityMessage.State = AbilityState.UNLOCKED;
        GameMessage.Send( _abilityMessage );
    }

    private void OnActivateButtonClick()
    {
        if( _selectedUnit.Abilities[ _ability.Index ] == AbilityState.UNLOCKED )
            _abilityMessage.State = AbilityState.ACTIVE;

        if( _selectedUnit.Abilities[ _ability.Index ] == AbilityState.ACTIVE )
            _abilityMessage.State = AbilityState.UNLOCKED;

        GameMessage.Send( _abilityMessage );
    }

    private void OnSelectedUnitChange( UnitModel value )
    {
        _selectedUnit = value;
        if( _ability != null )
            OnAbilityChange( _ability );
    }

    private void OnAbilityChange( AbilityData value )
    {
        _ability = value;
        _abilityMessage.Index = _ability.Index;
        NameText.text = _ability.Name;
        DescriptionText.text = "PRICE: " + _abilityPayment.GetUnlockAbilityPrice( _ability.Index );

        RemoveAllChildren( IncreaseEffectsGrid );
        RemoveAllChildren( DecreaseEffectsGrid );

        foreach( KeyValuePair<R, double> item in _ability.Effects )
        {
            if( item.Value > 0 )
                AddEffect( IncreaseEffectsGrid, item.Key, item.Value );
            else
                AddEffect( DecreaseEffectsGrid, item.Key, item.Value );
        }
        
        if( _selectedUnit != null && _selectedUnit.Abilities.ContainsKey( _ability.Index ) )
        {
            switch( _selectedUnit.Abilities[ _ability.Index ] )
            {
                case AbilityState.UNLOCKED:
                    UnlockButton.interactable = false;
                    ActivateButton.interactable = true;
                    _activateButtonText.text = "Activate";
                    break;
                case AbilityState.ACTIVE:
                    UnlockButton.interactable = false;
                    ActivateButton.interactable = true;
                    _activateButtonText.text = "Deactivate";
                    break;
                default:
                    UnlockButton.interactable = false;
                    ActivateButton.interactable = false;
                    break;
            }
        }
        else
        {
            UnlockButton.interactable = true;
            ActivateButton.interactable = false;
        }

        
    }

    private void AddEffect( Transform container, R type, double value )
    {
        GameObject go = Instantiate( EffectPrefab, container );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.SetProperty( type.ToString() );
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
