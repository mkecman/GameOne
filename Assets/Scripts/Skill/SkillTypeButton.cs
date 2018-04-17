using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UniRx;

public class SkillTypeButton : GameView
{
    public Toggle Button;
    public Text Label;

    private SkillTypeMessage _message = new SkillTypeMessage();

    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<SkillTypeMessage>( OnSkillTypeMessage );
        Button.OnValueChangedAsObservable().Where( _ => _ == true ).Subscribe( _ => GameMessage.Send( _message ) ).AddTo( disposables );
    }

    private void OnSkillTypeMessage( SkillTypeMessage value )
    {
        Button.isOn = value.Type == _message.Type;
    }

    internal void Setup( SkillType type )
    {
        _message.Type = type;
        Label.text = type.ToString();
    }
}
