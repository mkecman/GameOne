using PsiPhi;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OrganUnlockView : GameView
{
    public Button CraftButton;
    public UIPropertyView Name;
    public Transform ElementsGrid;
    public Transform EffectsGrid;
    public GameObject EffectPrefab;
    public Outline Outline;
    public Image BackgroundImage;
    public CompoundIconView compoundIcon;
    public Color[] StateColors = new Color[ 4 ] { Color.gray, Color.yellow, Color.green, Color.magenta };

    private CompoundJSON _compound;
    private OrganControlMessage _organControlMessage = new OrganControlMessage();
    private CompoundConfig _compoundConfig;
    private TreeBranchData _branch;
    private UnitModel _unit;

    void Awake()
    {
        GameMessage.Listen<CompoundSelectMessage>( OnCompoundSelected );
        _compoundConfig = GameConfig.Get<CompoundConfig>();
        GameModel.HandleGet<UnitModel>( OnUnitChange );
        CraftButton.OnClickAsObservable().Subscribe( _ => CraftCompound() ).AddTo( this );
    }

    private void CraftCompound()
    {
        GameMessage.Send( _organControlMessage );
    }

    private void OnCompoundSelected( CompoundSelectMessage value )
    {
        disposables.Clear();

        _compound = _compoundConfig[ value.Index ];
        _compound._CanCraft.Subscribe( _ => SetState( _ ? 1 : 0 ) ).AddTo( disposables );

        _organControlMessage.CompoundIndex = _compound.Index;
        _organControlMessage.Action = OrganControlAction.CRAFT;

        Name.SetProperty( _compound.Name );
        compoundIcon.Setup( _compound );

        _branch = _unit.OrganTree.GetBranch( value.Index );
        _branch._State.Subscribe( OnStateChange ).AddTo( disposables );

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

        RemoveAllChildren( EffectsGrid );
        foreach( KeyValuePair<R, float> effect in _compound.Effects )
        {
            AddEffect( effect.Key, effect.Value );
        }
    }

    private void OnStateChange( TreeBranchState state )
    {
        if( state == TreeBranchState.AVAILABLE )
            CraftButton.interactable = true;
        else
            CraftButton.interactable = false;
    }

    private void AddEffect( R type, float value )
    {
        GameObject go = Instantiate( EffectPrefab, EffectsGrid );
        UIPropertyView uipv = go.GetComponent<UIPropertyView>();
        uipv.StringFormat = "";
        uipv.SetProperty( type.ToString() );
        uipv.SetValue( float.MaxValue, value );
    }

    private void SetState( int colorIndex )
    {
        BackgroundImage.color = StateColors[ colorIndex ];
    }

    private void OnUnitChange( UnitModel value )
    {
        _unit = value;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _compound = null;
        _compoundConfig = null;
        _organControlMessage = null;
        _branch = null;
        _unit = null;
        GameModel.RemoveHandle<UnitModel>( OnUnitChange );
        GameMessage.StopListen<CompoundSelectMessage>( OnCompoundSelected );
    }


}
