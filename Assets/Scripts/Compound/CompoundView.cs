using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompoundView : GameView
{
    public Button MainButton;
    public UIPropertyView Name;
    public Transform ElementsGrid;
    public Transform EffectsGrid;
    public GameObject EffectPrefab;
    public Outline Outline;
    public Image BackgroundImage;
    public CompoundIconView compoundIcon;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private CompoundJSON _compound;
    private CompoundSelectMessage _compoundSelectMessage = new CompoundSelectMessage();
    private CompoundControlMessage _compoundControlMessage = new CompoundControlMessage( 0, CompoundControlAction.ADD );
    private IObservable<UniRx.Unit> _buttonStream;

    void Awake()
    {
        GameMessage.Listen<CompoundSelectMessage>( OnCompoundSelected );

        _buttonStream = MainButton.OnClickAsObservable();
        _buttonStream.Buffer( _buttonStream.Throttle( TimeSpan.FromMilliseconds( 300 ) ) )
            .Where( _ => _.Count >= 2 )
            .Subscribe( _ => CraftCompound() )
            .AddTo( this );
        _buttonStream.Subscribe( _ => SelectCompound() ).AddTo( this );
    }

    private void SelectCompound()
    {
        GameMessage.Send( _compoundSelectMessage );
    }

    private void CraftCompound()
    {
        GameMessage.Send( _compoundControlMessage );
    }

    private void OnCompoundSelected( CompoundSelectMessage value )
    {
        Outline.enabled = _compound.Index == value.Index;
        /*
        if( Outline.enabled )
            SetState( value.State );
            */
    }

    internal void SetState( int colorIndex )
    {
        BackgroundImage.color = StateColors[ colorIndex ];
    }

    internal void Setup( CompoundJSON compound )
    {
        disposables.Clear();

        _compound = compound;
        _compound._CanCraft.Subscribe( _ => SetState( _ ? 1 : 0 ) ).AddTo( disposables );
        _compoundSelectMessage.Index = _compound.Index;
        _compoundControlMessage.Index = _compound.Index;

        Name.SetProperty( _compound.Name );

        compoundIcon.Setup( _compound );

        for( int i = 0; i < ElementsGrid.childCount; i++ )
        {
            if( i >= _compound.Elements.Count )
            {
                ElementsGrid.GetChild( i ).GetComponent<CompoundElementAmountView>().Setup( null );
            }
            else
            {
                ElementsGrid.GetChild( i ).GetComponent<CompoundElementAmountView>().Setup( _compound.Elements[ i ] );
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
        _compoundControlMessage = null;
        _compoundSelectMessage = null;
        _buttonStream = null;
        GameMessage.StopListen<CompoundSelectMessage>( OnCompoundSelected );
    }
}
