using PsiPhi;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : AbstractController, IGameInit
{
    public UnitModel SelectedUnit { get { return _selectedUnit; } }

    private LifeModel _life;
    private GridModel<HexModel> _hexMapModel;
    private UnitModel _selectedUnit;

    private HexUpdateCommand _hexUpdateCommand;
    private UnitFactory _factory;
    private SkillCommand _skillCommand;

    private UnitModel _unitModel;

    public void Init()
    {
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
        _factory = GameModel.Get<UnitFactory>();
        _skillCommand = GameModel.Get<SkillCommand>();

        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }
    
    private void OnResistanceUpgrade( ResistanceUpgradeMessage value )
    {
        float delta = Mathf.Abs( value.Delta );
        if( _selectedUnit.Resistance[ value.Type ].ChangePosition( value.Delta ) )
            delta = -delta;

        _selectedUnit.AbilitiesDelta[ R.Science ].Value = _selectedUnit.AbilitiesDelta[ R.Science ].Value.Sum( delta );
    }
    
    public void AddUnit( int x, int y )
    {
        if( _hexMapModel.Table[ x ][ y ].Unit != null )
            return;

        UnitModel um = _factory.GetUnit( x, y );
        _hexMapModel.Table[ x ][ y ].Unit = um;
        _life.Units.Add( um );
        _life.Props[ R.Population ].Value++;
        SelectUnit( x, y );
    }

    public void RemoveUnit( UnitModel um )
    {
        if( _selectedUnit == um )
            DeselectUnit();

        _life.Units.Remove( um );
        _hexMapModel.Table[ um.X ][ um.Y ].Unit = null;
        _life.Props[ R.Population ].Value--;
    }

    public void MoveUnit( int xTo, int yTo )
    {
        HexModel hexModel = _hexMapModel.Table[ _selectedUnit.X ][ _selectedUnit.Y ];
        if( hexModel.Building != null )
            hexModel.Building.State = BuildingState.INACTIVE;

        hexModel.Unit = null;
        //_hexUpdateCommand.Execute( _life.Resistance, hexModel );

        _hexMapModel.Table[ xTo ][ yTo ].Unit = _selectedUnit;

        _selectedUnit.Props[ R.Altitude ].Value = _hexMapModel.Table[ xTo ][ yTo ].Props[ R.Altitude ].Value;
        _selectedUnit.X = xTo;
        _selectedUnit.Y = yTo;
    }
    
    public void DeselectUnit()
    {
        if( _selectedUnit != null )
        {
            _selectedUnit.isSelected.Value = false;
            _selectedUnit = null;
            GameModel.Set<UnitModel>( _selectedUnit );
        }
    }

    public void SelectUnit( int x, int y )
    {
        DeselectUnit();
        _selectedUnit = _hexMapModel.Table[ x ][ y ].Unit;
        _selectedUnit.isSelected.Value = true;
        _hexMapModel.Table[ x ][ y ].isExplored.Value = true;
        GameModel.Set<UnitModel>( _selectedUnit );
    }

    ///////////////////////// HANDLERS ////////////////////////////

    private void OnPlanetChange( PlanetModel value )
    {
        _hexMapModel = value.Map;
        _life = value.Life;

        GameMessage.Listen<HexClickedMessage>( OnHexClickedMessage );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
        GameMessage.Listen<ResistanceUpgradeMessage>( OnResistanceUpgrade );

        for( int i = 0; i < _life.Units.Count; i++ )
        {
            _unitModel = _life.Units[ i ];
            _unitModel.Props[ R.Altitude ].Value = _hexMapModel.Table[ _unitModel.X ][ _unitModel.Y ].Props[ R.Altitude ].Value; //not sure if this is needed? only if regenerating planets, but keeping units
            _unitModel.Y = _unitModel.Y; //update position vector3
            _hexMapModel.Table[ _unitModel.X ][ _unitModel.Y ].Unit = _unitModel;
        }

    }

    private void OnHexClickedMessage( HexClickedMessage value )
    {
        if( value.Hex.isMarked.Value )
            return;

        //Unit is in the clicked tile
        if( value.Hex.Unit != null )
            SelectUnit( value.Hex.X, value.Hex.Y );
        else
            DeselectUnit();

        GameModel.Set<HexModel>( value.Hex );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        //update all units active skills
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            _unitModel = _life.Units[ i ];
            for( int j = 0; j < _unitModel.ActiveSkills.Count; j++ )
            {
                _skillCommand.Execute( _unitModel, _unitModel.Skills[ _unitModel.ActiveSkills[ j ] ] );
            }
        }
    }

}
