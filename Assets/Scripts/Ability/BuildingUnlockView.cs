using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UniRx;

public class BuildingUnlockView : GameView, IPointerClickHandler
{
    public UIPropertyView AbilityUnlockCostText;
    public Transform EffectsGrid;
    public Outline Outline;
    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private BuildingModel _building;

    void Start()
    {
        GameMessage.Listen<BuildingUnlockSelected>( OnBuildingUnlockSelected );
        Debug.Log( "UNLOCK VIEW START" );
    }

    private void OnBuildingUnlockSelected( BuildingUnlockSelected value )
    {
        if( _building.Index == value.Index )
            Outline.enabled = true;
        else
            Outline.enabled = false;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send<BuildingUnlockSelected>( new BuildingUnlockSelected( _building.Index ) );
        //GameMessage.Send( new BuildingMessage( BuildingState.SELECTED, _building.Index ) );
    }

    internal void SetState()
    {
        if( _building.State == BuildingState.LOCKED || _building.State == BuildingState.UNLOCKED )
            BackgroundImage.color = StateColors[ (int)_building.State ];
    }

    internal void Setup( BuildingModel ability, GameObject effectPrefab )
    {
        _building = ability;

        disposables.Clear();
        _building._State.Subscribe( _ => SetState() ).AddTo( disposables );

        AbilityUnlockCostText.SetProperty( ability.Name );
        AbilityUnlockCostText.SetValue( ability.UnlockCost );

        foreach( KeyValuePair<string, double> item in _building.Effects )
        {
            AddEffect( effectPrefab, item.Key, item.Value );
        }
    }

    private void AddEffect( GameObject prefab, string type, double value )
    {
        GameObject go = Instantiate( prefab, EffectsGrid );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.SetProperty( type );
        uipv.SetValue( double.MaxValue, value );
    }
}
