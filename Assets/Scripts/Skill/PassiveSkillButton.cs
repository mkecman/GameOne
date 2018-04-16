using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class PassiveSkillButton : GameView
{
    public Button Button;
    public Text ButtonText;
    public SkillType Type;

    private SkillCommand _skillCommand;
    private UnitModel _unit;

    void Start()
    {
        _skillCommand = GameModel.Get<SkillCommand>();
        ButtonText.text = Type.ToString();
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        Button.OnClickAsObservable().Subscribe( _ => OnButtonClick() ).AddTo( disposables );
    }
    
    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
    }

    void OnDisable()
    {
        disposables.Clear();    
    }

    private void OnButtonClick()
    {
        if( _unit != null )
            _skillCommand.Execute( _unit, Type );
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        _skillCommand = null;
        _unit = null;
    }
}
