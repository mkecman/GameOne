using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class CompoundsControlPanel : GameView
{
    public Button UnlockButton;
    public Button ChooseButton;
    public Transform SkillTypeContainer;
    public GameObject SkillTypeButtonPrefab;
    public SkillUnlockPanel UnlockPanel;

    private UnitModel _unit;
    private SkillControlMessage _message = new SkillControlMessage();

    // Use this for initialization
    void Start()
    {
        UnlockButton.OnClickAsObservable().Subscribe( _ => OnUnlockButtonClick() );
        ChooseButton.OnClickAsObservable().Subscribe( _ => OnBuildButtonClick() );
    }

    private void OnEnable()
    {
        GameMessage.Send( new CameraControlMessage( false ) );
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        GameMessage.Listen<SkillTypeMessage>( OnSkillTypeMessage );
        GameMessage.Listen<SkillMessage>( OnSkillMessage );
    }

    private void OnDisable()
    {
        GameMessage.Send( new CameraControlMessage( true ) );
        GameMessage.StopListen<SkillTypeMessage>( OnSkillTypeMessage );
        GameMessage.StopListen<SkillMessage>( OnSkillMessage );
        GameModel.RemoveHandle<UnitModel>( OnUnitChange );
    }

    private void OnSkillTypeMessage( SkillTypeMessage value )
    {
        UnlockPanel.SetModel( _unit, value.Type );
    }

    private void OnSkillMessage( SkillMessage value )
    {
        _message.Index = value.Index;

        if( _unit.Skills.ContainsKey( value.Index ) )
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

    private void OnUnitChange( UnitModel value )
    {
        _unit = value;

        UnlockPanel.SetModel( _unit, SkillType.MINE );

        RemoveAllChildren( SkillTypeContainer );
        var list = _unit.Skills.Keys;
        foreach( SkillType item in list )
        {
            GameObject go = Instantiate( SkillTypeButtonPrefab, SkillTypeContainer );
            go.GetComponent<SkillTypeButton>().Setup( item );
        }
    }

    private void OnUnlockButtonClick()
    {
        _message.State = SkillState.UNLOCKED;
        GameMessage.Send( _message );
    }

    private void OnBuildButtonClick()
    {
        _message.State = SkillState.SELECTED;
        GameMessage.Send( _message );
        //gameObject.SetActive( false );
    }

}
