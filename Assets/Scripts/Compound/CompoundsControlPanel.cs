using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class CompoundsControlPanel : GameView
{
    public Button UnlockButton;
    public Button ChooseButton;
    public CompoundsPanel CompoundsPanel;

    private CompoundControlMessage _message = new CompoundControlMessage();
    private LifeModel _life;

    // Use this for initialization
    void Start()
    {
        UnlockButton.OnClickAsObservable().Subscribe( _ => OnUnlockButtonClick() );
        ChooseButton.OnClickAsObservable().Subscribe( _ => OnBuildButtonClick() );
    }

    private void OnEnable()
    {
        GameMessage.Send( new CameraControlMessage( false ) );
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<CompoundTypeMessage>( OnCompoundTypeMessage );
        GameMessage.Listen<SkillMessage>( OnSkillMessage );
    }

    private void OnDisable()
    {
        GameMessage.Send( new CameraControlMessage( true ) );
        GameMessage.StopListen<CompoundTypeMessage>( OnCompoundTypeMessage );
        GameMessage.StopListen<SkillMessage>( OnSkillMessage );
        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );
    }

    private void OnCompoundTypeMessage( CompoundTypeMessage value )
    {
        CompoundsPanel.SetModel( _life, value.Type );
    }

    private void OnSkillMessage( SkillMessage value )
    {
        _message.Index = value.Index;

        if( _life.Compounds.ContainsKey( value.Index ) )
        {
            UnlockButton.interactable = false;
            ChooseButton.interactable = true;
        }
        else
        {
            UnlockButton.interactable = true;
            ChooseButton.interactable = false;
        }
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;

        CompoundsPanel.SetModel( _life, CompoundType.Armor );
    }

    private void OnUnlockButtonClick()
    {
        //_message.State = SkillState.UNLOCKED;
        GameMessage.Send( _message );
    }

    private void OnBuildButtonClick()
    {
        //_message.State = SkillState.SELECTED;
        GameMessage.Send( _message );
        //gameObject.SetActive( false );
    }

}
