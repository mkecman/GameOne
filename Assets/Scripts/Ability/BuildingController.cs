using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BuildingController : AbstractController
{
    private PlanetModel _planet;
    private BuildingModel _selected;
    private BuildingPaymentService _pay;

    public BuildingController()
    {
        if( _pay == null )
            _pay = GameModel.Get<BuildingPaymentService>();

        GameMessage.Listen<BuildingMessage>( OnMessage );
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnMessage( BuildingMessage value )
    {
        switch( value.State )
        {
            case BuildingState.UNLOCKED:
                Unlock( value.Index );
                Select( value.Index );
                break;
            case BuildingState.ACTIVE:
                Activate( value.X, value.Y );
                break;
            case BuildingState.INACTIVE:
                Deactivate( value.X, value.Y );
                break;
            case BuildingState.BUILDING:
                Build( value.Index, value.X, value.Y );
                break;
            case BuildingState.DEMOLISH:
                Demolish( value.X, value.Y );
                break;
            case BuildingState.SELECTED:
                Select( value.Index );
                break;
            default:
                break;
        }
        
        //GameModel.Set<BuildingModel>( _planet.Map.Table[ value.X, value.Y ].Building );
    }

    private void Select( int index )
    {
        //_planet.Life.BuildingsState[ index ].State = BuildingState.SELECTED;
        GameModel.Set<BuildingModel>( _planet.Life.BuildingsState[ index ] );
    }

    private void Build( int index, int x, int y )
    {
        if( _pay.BuyBuild( index ) )
        {
            BuildingModel building = GameModel.Copy( _planet.Life.BuildingsState[ index ] );
            building.State = BuildingState.ACTIVE;
            building.X = x;
            building.Y = y;
            _planet.Life.Buildings.Add( building );
            _planet.Map.Table[ x, y ].Building = building;
            GameModel.Set<HexModel>( _planet.Map.Table[ x, y ] );
        }
    }

    private void Demolish( int x, int y )
    {
        _planet.Life.Buildings.Remove( _planet.Map.Table[ x, y ].Building );
        _planet.Map.Table[ x, y ].Building = null;
        GameModel.Set<HexModel>( _planet.Map.Table[ x, y ] );
    }
    
    private void Activate( int x, int y )
    {
        _planet.Map.Table[ x, y ].Building.State = BuildingState.ACTIVE;
    }

    private void Deactivate( int x, int y )
    {
        _planet.Map.Table[ x, y ].Building.State = BuildingState.INACTIVE;
    }

    private void Unlock( int index )
    {
        if( _pay.BuyUnlockAbility( index ) )
            _planet.Life.BuildingsState[ index ].State = BuildingState.UNLOCKED;
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
    }
    
}
