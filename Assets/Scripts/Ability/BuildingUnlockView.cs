using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public class BuildingUnlockView : GameView, IPointerClickHandler
{
    public UIPropertyView Name;
    public UIPropertyView UnlockPrice;
    public UIPropertyView BuildPrice;
    public UIPropertyView MaintenancePrice;
    public Transform EffectsGrid;
    public Outline Outline;
    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private BuildingModel _building;

    void Start()
    {
        GameMessage.Listen<BuildingUnlockMessage>( OnBuildingUnlockSelected );
    }

    private void OnBuildingUnlockSelected( BuildingUnlockMessage value )
    {
        if( _building.Index == value.Index )
            Outline.enabled = true;
        else
            Outline.enabled = false;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send<BuildingUnlockMessage>( new BuildingUnlockMessage( _building.Index ) );
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

        Name.SetProperty( ability.Name );
        UnlockPrice.SetProperty( "Unlock (Science):" );
        UnlockPrice.SetValue( Double.MaxValue, -ability.UnlockCost );
        BuildPrice.SetProperty( "Build (Minerals):" );
        BuildPrice.SetValue( Double.MaxValue, -ability.BuildCost );
        MaintenancePrice.SetProperty( "Maintenance (Minerals):" );
        MaintenancePrice.SetValue( Double.MaxValue, ability.Effects[ R.Minerals ] );

        var list = _building.Effects.Keys.ToList();
        list.Sort();

        foreach( var key in list )
        {
            if( key == R.Minerals )
                continue;

            AddEffect( effectPrefab, key, _building.Effects[ key ] );
        }
    }

    private void AddEffect( GameObject prefab, R type, double value )
    {
        GameObject go = Instantiate( prefab, EffectsGrid );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.SetProperty( type.ToString() );
        uipv.SetValue( double.MaxValue, value );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _building = null;
        GameMessage.StopListen<BuildingUnlockMessage>( OnBuildingUnlockSelected );
    }
}
