using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class SkillsControlPanel : GameView
{
    public Button UnlockButton;
    public Button ChooseButton;
    public Transform SkillTypeContainer;
    public GameObject SkillTypeButtonPrefab;
    public SkillUnlockPanel UnlockPanel;

    private UnitModel _unit;
    private SkillMessage _message = new SkillMessage();

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
        //OnBuildingChange( new BuildingUnlockMessage( _abilityMessage.Index ) );
    }

    private void OnDisable()
    {
        GameMessage.Send( new CameraControlMessage( true ) );
        GameMessage.StopListen<SkillTypeMessage>( OnSkillTypeMessage );
        GameModel.RemoveHandle<UnitModel>( OnUnitChange );
    }

    private void OnSkillTypeMessage( SkillTypeMessage value )
    {
        UnlockPanel.SetModel( _unit, value.Type );
    }

    private void OnSkillMessage( SkillMessage value )
    {
        if( _unit.Skills[ value.Type ].State == SkillState.LOCKED )
        {
            UnlockButton.interactable = true;
            ChooseButton.interactable = false;
        }
        else
        {
            UnlockButton.interactable = false;
            ChooseButton.interactable = true;
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
