using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Units
{
    private LifeModel _life;
    private GridModel<HexModel> _hexMapModel;
    
    private GridModel<UnitModel> _unitMap;
    private UnitModel _selectedUnit;
    
    private List<HexModel> _markedHexes;

    public void Load( PlanetModel planet )
    {
        _hexMapModel = planet.Map;
        _life = planet.Life;
        _unitMap = new GridModel<UnitModel>( planet.Map.Width, planet.Map.Height );
        _markedHexes = new List<HexModel>();

        GameMessage.Listen<HexClickedMessage>( OnHexClickedMessage );
        GameMessage.Listen<UnitMessage>( OnUnitMessage );
    }
    
    public void UpdateStep()
    {
        double food = 0;
        double science = 0;
        double words = 0;

        UnitModel um;
        ElementModel em;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            um = _life.Units[ i ];
            em = _hexMapModel.Table[ um.X.Value, um.Y.Value ].Element;
            food += em.Modifier( ElementModifiers.Food ).Delta;
            science += em.Modifier( ElementModifiers.Science ).Delta;
            words += em.Modifier( ElementModifiers.Words ).Delta;
        }

        _life.Food += food;
        _life.Science += science;
        _life.Words += words;
    }

    private void OnUnitMessage( UnitMessage value )
    {
        if( value.Type == UnitMessageType.Add )
            AddUnit( value.X, value.Y );
        else
            MoveUnit( value.X, value.Y );
    }

    private void AddUnit( int x, int y )
    {
        if( _unitMap.Table[ x, y ] != null )
            return;
        
        UnitModel um = new UnitModel( x, y, _hexMapModel.Table[ x, y ].Altitude );
        _unitMap.Table[ x, y ] = um;
        _life.Units.Add( um );
    }

    private void MoveUnit( int xTo, int yTo )
    {
        _unitMap.Table[ _selectedUnit.X.Value, _selectedUnit.Y.Value ] = null;
        _unitMap.Table[ xTo, yTo ] = _selectedUnit;
        _selectedUnit.Altitude.Value = _hexMapModel.Table[ xTo, yTo ].Altitude;
        _selectedUnit.X.Value = xTo;
        _selectedUnit.Y.Value = yTo;
    }

    private void OnHexClickedMessage( HexClickedMessage value )
    {
        int x = value.Hex.X;
        int y = value.Hex.Y;

        if( _selectedUnit != null && _hexMapModel.Table[ x, y ].isMarked.Value == true )
        {
            MoveUnit( x, y );
            SelectUnit( x, y );
        }

        //Unit is in the clicked tile
        if( _unitMap.Table[ x, y ] != null )
        {
            SelectUnit( x, y );
        }
        else
        {
            DeselectUnit();
        }
    }

    private void DeselectUnit()
    {
        if( _selectedUnit != null )
        {
            UnmarkHexes();
            _selectedUnit.isSelected.Value = false;
        }
    }

    private void SelectUnit( int x, int y )
    {
        DeselectUnit();
        _selectedUnit = _unitMap.Table[ x, y ];
        _selectedUnit.isSelected.Value = true;
        MarkMoveHexes( x, y );
    }

    private void MarkMoveHexes( int x, int y )
    {
        UnmarkHexes();

        CheckAndMark( x, y + 1 );

        CheckAndMark( x - 1, y );
        CheckAndMark( x, y );
        CheckAndMark( x + 1, y );

        CheckAndMark( x, y - 1 );

        if( y % 2 == 0 )
        {
            CheckAndMark( x - 1, y + 1 );
            CheckAndMark( x - 1, y - 1 );
        }
        else
        {
            CheckAndMark( x + 1, y + 1 );
            CheckAndMark( x + 1, y - 1 );
        }
    }

    private void CheckAndMark( int x, int y )
    {
        if( x >= 0 && y >= 0 && x < _hexMapModel.Width && y < _hexMapModel.Height )
        {
            if( _unitMap.Table[ x, y ] == null )
            {
                _hexMapModel.Table[ x, y ].isMarked.Value = true;
                _markedHexes.Add( _hexMapModel.Table[ x, y ] );
            }
        }
    }

    private void UnmarkHexes()
    {
        for( int i = 0; i < _markedHexes.Count; i++ )
            _markedHexes[ i ].isMarked.Value = false;

        _markedHexes.Clear();
    }
}
