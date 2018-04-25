using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UniRx;

public class CompoundTypeButton : GameView
{
    public CompoundType Type;
    public Toggle Button;
    public Text Label;

    private CompoundTypeMessage _message = new CompoundTypeMessage();

    // Use this for initialization
    void Start()
    {
        _message.Type = Type;
        Label.text = Type.ToString();

        GameMessage.Listen<CompoundTypeMessage>( OnCompoundTypeMessage );
        Button.OnValueChangedAsObservable().Where( _ => _ == true ).Subscribe( _ => GameMessage.Send( _message ) ).AddTo( disposables );
    }

    private void OnCompoundTypeMessage( CompoundTypeMessage value )
    {
        Button.isOn = value.Type == _message.Type;
    }
}
