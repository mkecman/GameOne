using System;
using UniRx;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveSkillButton : GameView
{
    public Toggle Button;
    public Text ButtonText;
    public int Index;

    private SkillCommand _skillCommand;
    private CompoundPaymentService _pay;
    private UnitModel _unit;
    private TooltipMessage _tooltipMessage = new TooltipMessage();
    private SkillDeactivateAllMessage _deactivateMessage = new SkillDeactivateAllMessage();
    private LifeModel _life;
    private int _compoundIndex;
    private int _youHave = 0;

    void Start()
    {
        _skillCommand = GameModel.Get<SkillCommand>();
        _pay = GameModel.Get<CompoundPaymentService>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameModel.HandleGet<UnitModel>( OnUnitChange );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );
        GameModel.RemoveHandle<UnitModel>( OnUnitChange );
        GameMessage.StopListen<SkillDeactivateAllMessage>( OnSkillDeactivate );

        _tooltipMessage.Action = TooltipAction.HIDE;
        GameMessage.Send( _tooltipMessage );

        _life = null;
        _skillCommand = null;
        _pay = null;
        _tooltipMessage = null;
        _deactivateMessage = null;
        _unit = null;
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
    }

    private void OnUnitChange( UnitModel value )
    {
        //Function delegate race condition destroys this object after the handler has already been called
        //if disposables is null, it means this objecg has been destroyed
        if( disposables == null )
            return;

        disposables.Clear();
        _unit = value;

        if( _unit != null )
        {
            _compoundIndex = (int)_unit.Skills[ Index ].UseCost;
            if( _life.Compounds.ContainsKey( _compoundIndex ) )
                _life.Compounds[ _compoundIndex ].Subscribe( _ => _youHave = _ ).AddTo( disposables );
            //_unit.Skills[ Index ]._State.Subscribe( _ => OnToggleValueChanged() ).AddTo( disposables );
            Button.OnValueChangedAsObservable().Subscribe( _ => OnToggleValueChanged() ).AddTo( disposables );
            ButtonText.text = _unit.Skills[ Index ].Type.ToString();
        }
    }

    private void OnSkillDeactivate( SkillDeactivateAllMessage value )
    {
        GameMessage.StopListen<SkillDeactivateAllMessage>( OnSkillDeactivate );
        Button.isOn = false;
        _tooltipMessage.Action = TooltipAction.HIDE;
        GameMessage.Send( _tooltipMessage );
    }

    private void OnToggleValueChanged()
    {
        if( Button.isOn )
        {
            GameMessage.Send( _deactivateMessage );

            GameMessage.Listen<SkillDeactivateAllMessage>( OnSkillDeactivate );
            _skillCommand.Execute( _unit, Index );

            _tooltipMessage.Text = "Cost: " + _pay.GetSkillPriceText( (int)_unit.Skills[ Index ].UseCost, 1 ) + "\nYou have: " + _youHave;
            _tooltipMessage.Position = transform.position;
            _tooltipMessage.Action = TooltipAction.SHOW;
        }
        else
        {
            _tooltipMessage.Action = TooltipAction.HIDE;
        }
        GameMessage.Send( _tooltipMessage );
    }
}
