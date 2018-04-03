using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UniRx;

public class BuildingUnlockPanel : GameView
{
    public Button UnlockButton;
    public Button BuildButton;
    public Transform Container;
    public GameObject AbilityUnlockPrefab;
    public GameObject EffectPrefab;

    private BuildingMessage _abilityMessage;
    private LifeModel _life;

    private void OnEnable()
    {
        GameMessage.Send( new CameraControlMessage( false ) );
        GameMessage.Listen<BuildingUnlockMessage>( OnBuildingChange );
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        OnBuildingChange( new BuildingUnlockMessage( _abilityMessage.Index ) );
    }

    private void OnDisable()
    {
        GameMessage.Send( new CameraControlMessage( true ) );
        GameMessage.StopListen<BuildingUnlockMessage>( OnBuildingChange );
        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );
    }

    // Use this for initialization
    void Awake()
    {
        _abilityMessage = new BuildingMessage( BuildingState.LOCKED, 0 );
        UnlockButton.OnClickAsObservable().Subscribe( _ => OnUnlockButtonClick() );
        BuildButton.OnClickAsObservable().Subscribe( _ => OnBuildButtonClick() );
    }

    private void OnUnlockButtonClick()
    {
        _abilityMessage.State = BuildingState.UNLOCKED;
        GameMessage.Send( _abilityMessage );
    }

    private void OnBuildButtonClick()
    {
        _abilityMessage.State = BuildingState.BUILDING;
        GameMessage.Send( _abilityMessage );
        gameObject.SetActive( false );
    }

    private void OnBuildingChange( BuildingUnlockMessage value )
    {
        _abilityMessage.Index = value.Index;
        HexModel hexModel = GameModel.Get<HexModel>();
        _abilityMessage.X = hexModel.X;
        _abilityMessage.Y = hexModel.Y;

        if( _life.BuildingsState[ value.Index ].State == BuildingState.LOCKED )
        {
            UnlockButton.interactable = true;
            BuildButton.interactable = false;
        }
        else
        {
            UnlockButton.interactable = false;
            BuildButton.interactable = true;
        }
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
        RemoveAllChildren( Container );
        for( int i = 0; i < _life.BuildingsState.Count; i++ )
        {
            GameObject ability_go = Instantiate( AbilityUnlockPrefab, Container );
            BuildingUnlockView abilityUnlockView = ability_go.GetComponent<BuildingUnlockView>();
            abilityUnlockView.Setup( _life.BuildingsState[ i ], EffectPrefab );
        }
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
