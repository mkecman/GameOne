using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;
using System.Collections.Generic;

public class UnitInfoPanel : GameView
{
    public GameObject PropertyPrefab;

    private UnitModel _unit;
    private UIPropertyView _ui;

    private Dictionary<R, UIPropertyView> _propViews;

    // Use this for initialization
    void Start()
    {
        _propViews = new Dictionary<R, UIPropertyView>();
        GameModel.HandleGet<UnitModel>( OnModelChange );
    }

    private void OnModelChange( UnitModel value )
    {
        if( value != null && value != _unit )
        {
            Clear();
            _unit = value;

            AddProp( R.Health );
            AddProp( R.Experience );
            AddProp( R.Attack );
            AddProp( R.Armor );
            AddProp( R.Body );
            AddProp( R.Mind );
            AddProp( R.Soul );
            AddProp( R.Speed );
        }
        else
        {
            Clear();
            _unit = null;
        }
    }

    private void Clear()
    {
        disposables.Clear();
        _propViews.Clear();
        RemoveAllChildren( transform );
    }

    private void AddProp( R prop )
    {
        _ui = Instantiate( PropertyPrefab, transform ).GetComponent<UIPropertyView>();
        _ui.SetProperty( prop.ToString() );
        _ui.SetValue( _unit.Props[ prop ].Value );
        _propViews.Add( prop, _ui );
        _unit.Props[ prop ]._Value.Subscribe( _ => _propViews[ prop ].SetValue( _ ) ).AddTo( disposables );
    }
}
