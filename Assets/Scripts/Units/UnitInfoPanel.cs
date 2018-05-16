using System.Collections.Generic;
using UnityEngine;

public class UnitInfoPanel : GameView
{
    public Transform Container;
    public GameObject PropertyPrefab;

    private UnitModel _unit;
    private UnitPropUpgradeView _ui;

    private Dictionary<R, UnitPropUpgradeView> _propViews;

    // Use this for initialization
    void Start()
    {
        _propViews = new Dictionary<R, UnitPropUpgradeView>();
        GameModel.HandleGet<UnitModel>( OnModelChange );
        GameMessage.Send( new PanelMessage( PanelAction.HIDE, PanelNames.UnitEditPanel ) );
    }

    private void OnModelChange( UnitModel value )
    {
        if( _unit == value )
            return;

        if( value != null )
        {
            Clear();
            _unit = value;

            AddProp( R.Health, false, "N0", true );
            AddProp( R.Experience, false, "N0", true );
            AddProp( R.Level );
            AddProp( R.UpgradePoint );
            AddProp( R.Body, true );
            AddProp( R.Mind, true );
            AddProp( R.Soul, true );
            AddProp( R.Armor, false, "##%" );
            AddProp( R.Attack );
            AddProp( R.Speed, false, "##%" );
            AddProp( R.Critical, false, "##%" );

            GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.UnitEditPanel ) );
        }
        else
        {
            GameMessage.Send( new PanelMessage( PanelAction.HIDE, PanelNames.UnitEditPanel ) );
            Clear();
            _unit = null;
        }
    }

    private void Clear()
    {
        disposables.Clear();
        _propViews.Clear();
        RemoveAllChildren( Container );
    }

    private void AddProp( R prop, bool canChange = false, string stringFormat = "N0", bool showMaxValue = false )
    {
        _ui = Instantiate( PropertyPrefab, Container ).GetComponent<UnitPropUpgradeView>();
        _ui.SetModel( prop, _unit, canChange, stringFormat, showMaxValue );

        _propViews.Add( prop, _ui );
    }
}
