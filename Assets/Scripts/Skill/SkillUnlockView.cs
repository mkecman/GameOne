using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public class SkillUnlockView : GameView, IPointerClickHandler
{
    public UIPropertyView Name;
    public UIPropertyView UnlockPrice;
    public UIPropertyView MaintenancePrice;
    public Transform EffectsGrid;
    public Outline Outline;
    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private SkillData _skill;
    private SkillMessage _message = new SkillMessage();

    void Start()
    {
        GameMessage.Listen<SkillMessage>( OnSkillSelected );
    }

    private void OnSkillSelected( SkillMessage value )
    {
        Outline.enabled = _skill.Index == value.Index;
        if( Outline.enabled )
            SetState( value.State );
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send( _message );
    }

    internal void SetState( SkillState state )
    {
        //if( _skill.State == SkillState.LOCKED || _skill.State == SkillState.UNLOCKED )
            BackgroundImage.color = StateColors[ (int)state ];
    }

    internal void Setup( SkillData skill, GameObject effectPrefab )
    {
        _skill = skill;
        _message.Index = _skill.Index;
        _message.State = _skill.State;

        //disposables.Clear();
        //_skill._State.Subscribe( _ => SetState() ).AddTo( disposables );
        SetState( _skill.State );

        Name.SetProperty( _skill.Name );
        UnlockPrice.SetProperty( "Unlock:" );
        UnlockPrice.SetValue( float.MaxValue, -_skill.UnlockCost );

        foreach( KeyValuePair<R, float> effect in _skill.Effects )
        {
            if( effect.Value != 0 )
                AddEffect( effectPrefab, effect.Key, effect.Value );
        }
    }

    private void AddEffect( GameObject prefab, R type, float value )
    {
        GameObject go = Instantiate( prefab, EffectsGrid );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.StringFormat = "";
        uipv.SetProperty( type.ToString() );
        uipv.SetValue( float.MaxValue, value );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _skill = null;
        _message = null;
        GameMessage.StopListen<SkillMessage>( OnSkillSelected );
    }
}
