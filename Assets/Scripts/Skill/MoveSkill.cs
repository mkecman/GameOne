﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveSkill : ISkill
{
    private UnitPaymentService _pay;
    private UnitController _controller;
    private PlanetController _planetController;
    private List<HexModel> _markedHexes;
    private GridModel<HexModel> _hexMapModel;
    private List<Vector2Int> _positions;
    private SkillDeactivateAllMessage _deactivateMessage = new SkillDeactivateAllMessage();

    public void Init()
    {
        _pay = GameModel.Get<UnitPaymentService>();
        _controller = GameModel.Get<UnitController>();
        _planetController = GameModel.Get<PlanetController>();
        _markedHexes = new List<HexModel>();
        GameMessage.Listen<SkillDeactivateAllMessage>( OnSkillDeactivate );
    }
    
    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        _hexMapModel = _planetController.SelectedPlanet.Map;
        GameMessage.Send( _deactivateMessage );
        MarkNeighborHexes( unitModel );
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        if( value.Hex.isMarked.Value )
        {
            if( _pay.BuyMoveUnit() )
            {
                _controller.MoveUnit( value.Hex.X, value.Hex.Y );
                UnmarkHexes();
                MarkNeighborHexes( _controller.SelectedUnit );
            }
            else
            {
                Deactivate();
            }
        }
        else
        {
            Deactivate();
        }
    }

    private void OnSkillDeactivate( SkillDeactivateAllMessage value )
    {
        Deactivate();
    }

    private void Deactivate()
    {
        UnmarkHexes();
        GameMessage.StopListen<HexClickedMessage>( OnHexClicked );
    }

    private void MarkNeighborHexes( UnitModel unit )
    {
        _positions = HexMapHelper.GetPositions( unit.X, unit.Y, _hexMapModel.Width, _hexMapModel.Height );
        for( int i = 0; i < _positions.Count; i++ )
            CheckAndMark( _positions[ i ].x, _positions[ i ].y, unit );
    }

    HexModel _hex;
    private void CheckAndMark( int x, int y, UnitModel unit )
    {
        _hex = _hexMapModel.Table[ x ][ y ];
        if( _hex.Unit == null && Math.Abs( _hex.Props[ R.Altitude ].Value - unit.Props[ R.Altitude ].Value ) <= .5 ) //check if it can climb
        {
            _hex.isMarked.Value = true;
            _hex.isExplored.Value = true;
            _markedHexes.Add( _hex );
        }
    }

    private void UnmarkHexes()
    {
        for( int i = 0; i < _markedHexes.Count; i++ )
            _markedHexes[ i ].isMarked.Value = false;

        _markedHexes.Clear();
    }
}