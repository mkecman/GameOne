using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class ActiveSkillButton : GameView
{
    public Button Button;
    public Text ButtonText;
    public int Index;

    private SkillCommand _skillCommand;
    private UnitModel _unit;

    void Start()
    {
        _skillCommand = GameModel.Get<SkillCommand>();
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        Button.OnClickAsObservable().Subscribe( _ => OnButtonClick() ).AddTo( disposables );
    }
    
    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
        if( _unit != null )
            ButtonText.text = _unit.Skills[ Index ].Type.ToString();
    }

    void OnDisable()
    {
        disposables.Clear();    
    }

    private void OnButtonClick()
    {
        if( _unit != null )
            _skillCommand.Execute( _unit, Index );
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        _skillCommand = null;
        _unit = null;
    }
}
