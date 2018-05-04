using System;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : ISkill
{
    private CompoundPaymentService _pay;
    private UnitController _controller;
    private PlanetController _planetController;
    private List<HexModel> _markedHexes;
    private GridModel<HexModel> _hexMapModel;
    private List<Vector2Int> _positions;
    private HexModel _hex;
    private SkillData _skillData;


    public void Init()
    {
        _pay = GameModel.Get<CompoundPaymentService>();
        _controller = GameModel.Get<UnitController>();
        _planetController = GameModel.Get<PlanetController>();
        _markedHexes = new List<HexModel>();
        GameMessage.Listen<SkillDeactivateAllMessage>( OnSkillDeactivate );
    }

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        if( _pay.BuySkillUse( (int)skillData.UseCost, 1, false ) )
        {
            _skillData = skillData;
            _skillData.State = SkillState.EXECUTING;
            _hexMapModel = _planetController.SelectedPlanet.Map;
            MarkNeighborHexes( unitModel );
            GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        }
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        if( value.Hex.isMarked.Value )
        {
            if( _pay.BuySkillUse( (int)_skillData.UseCost, 1, true ) )
                _controller.AddUnit( value.Hex.X, value.Hex.Y );
        }

        Deactivate();
    }

    private void OnSkillDeactivate( SkillDeactivateAllMessage value )
    {
        Deactivate();
    }

    private void Deactivate()
    {
        UnmarkHexes();
        GameMessage.StopListen<HexClickedMessage>( OnHexClicked );

        if( _skillData != null )
        {
            _skillData.State = SkillState.UNLOCKED;
            _skillData = null;
        }
    }

    private void MarkNeighborHexes( UnitModel unit )
    {
        _positions = HexMapHelper.GetPositions( unit.X, unit.Y, _hexMapModel.Width, _hexMapModel.Height );
        for( int i = 0; i < _positions.Count; i++ )
            CheckAndMark( _positions[ i ].x, _positions[ i ].y, unit );
    }

    private void CheckAndMark( int x, int y, UnitModel unit )
    {
        _hex = _hexMapModel.Table[ x ][ y ];
        if( _hex.Unit == null )
        {
            _hex.isMarked.Value = true;
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
