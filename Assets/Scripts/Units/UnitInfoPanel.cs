﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;
using System.Collections.Generic;

public class UnitInfoPanel : GameView
{
    public GameObject PropertyPrefab;

    private UnitModel _unit;
    private UnitPropUpgradeView _ui;

    private Dictionary<R, UnitPropUpgradeView> _propViews;

    // Use this for initialization
    void Start()
    {
        _propViews = new Dictionary<R, UnitPropUpgradeView>();
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
            AddProp( R.Level );
            AddProp( R.UpgradePoint );
            AddProp( R.Attack );
            AddProp( R.Armor );
            AddProp( R.Body, true );
            AddProp( R.Mind, true );
            AddProp( R.Soul, true );
            AddProp( R.Speed, true );
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

    private void AddProp( R prop, bool canChange = false )
    {
        _ui = Instantiate( PropertyPrefab, transform ).GetComponent<UnitPropUpgradeView>();
        _ui.SetModel( prop, _unit, canChange );
        
        _propViews.Add( prop, _ui );
    }
}
