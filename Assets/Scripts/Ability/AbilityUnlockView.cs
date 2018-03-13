using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AbilityUnlockView : GameView, IPointerClickHandler
{
    public UIPropertyView AbilityUnlockCostText;
    public Transform EffectsGrid;

    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private AbilityData abilityData;

    void Start()
    {
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    private void OnUnitChange( UnitModel value )
    {
        if( value != null && value.Abilities.ContainsKey( abilityData.Index ) )
            SetState( value.Abilities[ abilityData.Index ] );
        else
            SetState( AbilityState.LOCKED );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send( new AbilityMessage( AbilityState.SELECTED, abilityData.Index ) );
    }

    internal void SetState( AbilityState state )
    {
        BackgroundImage.color = StateColors[ (int)state ];
    }

    internal void Setup( AbilityData ability, GameObject effectPrefab )
    {
        abilityData = ability;
        AbilityUnlockCostText.SetProperty( ability.Name );
        AbilityUnlockCostText.SetValue( ability.UnlockCost );

        foreach( KeyValuePair<R, double> item in abilityData.Effects )
        {
            AddEffect( effectPrefab, item.Key, item.Value );
        }
        
        SetState( AbilityState.LOCKED );
    }

    private void AddEffect( GameObject prefab, R type, double value )
    {
        GameObject go = Instantiate( prefab, EffectsGrid );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.SetProperty( type.ToString() );
        uipv.SetValue( double.MaxValue, value );
    }
}
