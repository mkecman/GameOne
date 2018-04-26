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
    public GameObject EffectPrefab;
    public Outline Outline;
    public Image BackgroundImage;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private CompoundJSON _compound;
    private CompoundSelectMessage _message = new CompoundSelectMessage();
    private LifeModel _life;

    void Awake()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<CompoundSelectMessage>( OnCompoundSelected );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
    }

    private void OnCompoundSelected( CompoundSelectMessage value )
    {
        Outline.enabled = _compound.Index == value.Index;
        /*
        if( Outline.enabled )
            SetState( value.State );
            */
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        GameMessage.Send( _message );
    }

    internal void SetState( int colorIndex )
    {
        Debug.Log( _compound.Name + "----------- SetState: " + colorIndex );
        BackgroundImage.color = StateColors[ colorIndex ];
    }

    internal void Setup( CompoundJSON compound )
    {
        disposables.Clear();

        _compound = compound;
        _compound._CanCraft.Subscribe( _ => SetState( _ ? 1 : 0 ) ).AddTo( disposables );
        _message.Index = _compound.Index;

        Name.SetProperty( _compound.Name );

        for( int i = 0; i < ElementsGrid.childCount; i++ )
        {
            if( i >= _compound.Elements.Count )
            {
                ElementsGrid.GetChild( i ).GetComponent<CompoundElementAmountView>().Setup( null );
            }
            else
            {
                ElementsGrid.GetChild(i).GetComponent<CompoundElementAmountView>().Setup( _compound.Elements[ i ] );
            }
        }
        
        foreach( KeyValuePair<R, float> effect in _compound.Effects )
        {
            AddEffect( effect.Key, effect.Value );
        }
    }

    private void AddEffect( R type, float value )
    {
        GameObject go = Instantiate( EffectPrefab, EffectsGrid );
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
        GameMessage.StopListen<CompoundSelectMessage>( OnCompoundSelected );
    }
}
