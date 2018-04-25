using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public class CompoundView : GameView, IPointerClickHandler
{
    public UIPropertyView Name;
    public Transform ElementsGrid;
    public Transform EffectsGrid;
    public Outline Outline;
    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private CompoundJSON _compound;
    private SkillMessage _message = new SkillMessage();
    private LifeModel _life;

    void Awake()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<SkillMessage>( OnSkillSelected );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
    }

    private void OnSkillSelected( SkillMessage value )
    {
        Outline.enabled = _compound.Index == value.Index;
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

    internal void Setup( CompoundJSON compound, GameObject effectPrefab )
    {
        _compound = compound;
        _message.Index = _compound.Index;

        Name.SetProperty( _compound.Name );

        for( int i = 0; i < ElementsGrid.childCount; i++ )
        {
            if( i >= _compound.Elements.Count )
            {
                ElementsGrid.GetChild( i ).GetComponent<CompoundElementAmountView>().Setup( null, 0 );
            }
            else
            {
                ElementsGrid.GetChild(i).GetComponent<CompoundElementAmountView>().Setup( _life.Elements[ _compound.Elements[ i ].Index ], _compound.Elements[ i ].Amount );
            }
        }
        
        foreach( KeyValuePair<R, float> effect in _compound.Effects )
        {
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
        _compound = null;
        _message = null;
        GameMessage.StopListen<SkillMessage>( OnSkillSelected );
    }
}
