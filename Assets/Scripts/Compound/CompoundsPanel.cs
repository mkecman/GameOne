using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UniRx;

public class CompoundsPanel : GameView
{
    public Transform Container;
    public GameObject CompoundUnlockPrefab;
    public Button UnlockButton;
    public Button ChooseButton;

    private CompoundConfig _compoundConfig;
    private List<CompoundViewModel> _compounds;
    private CompoundJSON _compound;
    private CompoundControlMessage _message = new CompoundControlMessage();
    private LifeModel _life;

    // Use this for initialization
    void Awake()
    {
        _compoundConfig = GameModel.Copy( GameConfig.Get<CompoundConfig>() );
        _compounds = new List<CompoundViewModel>();
        for( int i = 0; i < _compoundConfig.Count; i++ )
            _compounds.Add( new CompoundViewModel( _compoundConfig[i] ) );

        UnlockButton.OnClickAsObservable().Subscribe( _ => OnUnlockButtonClick() ).AddTo( this );
        ChooseButton.OnClickAsObservable().Subscribe( _ => OnBuildButtonClick() ).AddTo( this );
    }

    private void OnEnable()
    {
        GameMessage.Send( new CameraControlMessage( false ) );
        GameMessage.Listen<CompoundTypeMessage>( OnCompoundTypeMessage );
        GameMessage.Listen<CompoundSelectMessage>( OnCompoundSelectMessage );
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Setup( value.Life.Elements );

        SetModel( CompoundType.Armor );
    }

    private void OnDisable()
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Disable();

        disposables.Clear();
        canCraftDisposable.Clear();
        RemoveAllChildren( Container );

        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );
        GameMessage.Send( new CameraControlMessage( true ) );
        GameMessage.StopListen<CompoundTypeMessage>( OnCompoundTypeMessage );
        GameMessage.StopListen<CompoundSelectMessage>( OnCompoundSelectMessage );
    }

    private void SetModel( CompoundType type )
    {
        RemoveAllChildren( Container );
        for( int i = 0; i < _compoundConfig.Count; i++ )
        {
            _compound = _compoundConfig[ i ];
            if( _compound.Type == type )
            {
                GameObject go = Instantiate( CompoundUnlockPrefab, Container );
                go.GetComponent<CompoundView>().Setup( _compound );
            }
        }
    }

    private void OnCompoundTypeMessage( CompoundTypeMessage value )
    {
        SetModel( value.Type );
    }

    private CompositeDisposable canCraftDisposable = new CompositeDisposable();
    private void OnCompoundSelectMessage( CompoundSelectMessage value )
    {
        _message.Index = value.Index;
        canCraftDisposable.Clear();
        _compoundConfig[ _message.Index ]._CanCraft.Subscribe( _ => ChooseButton.interactable = _ ).AddTo( canCraftDisposable );
    }

    private void OnUnlockButtonClick()
    {
        //_message.State = SkillState.UNLOCKED;
        //GameMessage.Send( _message );
    }

    private void OnBuildButtonClick()
    {
        //_message.State = SkillState.SELECTED;
        GameMessage.Send( _message );
    }

}
