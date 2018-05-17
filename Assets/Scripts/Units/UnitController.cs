using PsiPhi;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class UnitController : AbstractController, IGameInit
{
    public UnitModel SelectedUnit { get { return _selectedUnit; } }

    private LifeModel _life;
    private GridModel<HexModel> _hexMapModel;
    private UnitModel _selectedUnit;

    private UnitFactory _factory;
    private SkillCommand _skillCommand;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;
    private UnitUseCompoundCommand _unitUseCompoundCommand;
    private UnitModel _tempUnitModel;
    private HexModel _tempHexModel;

    public void Init()
    {
        _factory = GameModel.Get<UnitFactory>();
        _skillCommand = GameModel.Get<SkillCommand>();
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
        _unitUseCompoundCommand = GameModel.Get<UnitUseCompoundCommand>();

        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<UnitPropUpgradeMessage>( OnUnitPropUpgradeMessage );
        GameMessage.Listen<UnitUseCompoundMessage>( OnUnitUseCompoundMessage );
        GameMessage.Listen<UnitSelectMessage>( OnUnitSelectMessage );
    }

    private void OnUnitSelectMessage( UnitSelectMessage value )
    {
        SelectUnit( value.X, value.Y );
    }

    private void OnUnitUseCompoundMessage( UnitUseCompoundMessage value )
    {
        _unitUseCompoundCommand.Execute( value.Unit, value.CompoundIndex );
    }

    private void OnUnitPropUpgradeMessage( UnitPropUpgradeMessage value )
    {
        if( _selectedUnit.Props[R.UpgradePoint].Value > 0 )
        {
            _selectedUnit.Props[ R.UpgradePoint ].Value--;
            _selectedUnit.Props[ value.Property ].Value++;
        }
    }

    private void OnResistanceUpgrade( ResistanceUpgradeMessage value )
    {
        float delta = Mathf.Abs( value.Delta );
        if( _selectedUnit.Resistance[ value.Type ].ChangePosition( value.Delta ) )
            delta = -delta;

        _unitDefenseUpdateCommand.Execute( _selectedUnit );
        //_selectedUnit.AbilitiesDelta[ R.Science ].Value = _selectedUnit.AbilitiesDelta[ R.Science ].Value.Sum( delta );
    }

    public void AddUnit( int x, int y )
    {
        if( _hexMapModel.Table[ x ][ y ].Unit != null )
            return;

        _tempUnitModel = _factory.GetUnit( x, y );
        _hexMapModel.Table[ x ][ y ].Unit = _tempUnitModel;
        _unitDefenseUpdateCommand.Execute( _tempUnitModel );

        _life.Units.Add( _tempUnitModel );
        _life.Props[ R.Population ].Value++;

        SelectUnit( x, y );
    }

    public void RemoveUnit( UnitModel um )
    {
        if( _selectedUnit == um )
            DeselectUnit( false );

        um.Dispose();
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
        _hexMapModel.Table[ xTo ][ yTo ].isExplored.Value = true;
        _unitDefenseUpdateCommand.Execute( _selectedUnit );
    }
    
    public void DeselectUnit( bool setUnitModel = true )
    {
        if( _selectedUnit != null )
        {
            _selectedUnit.isSelected.Value = false;
            _selectedUnit = null;
            if( setUnitModel )
                GameModel.Set<UnitModel>( _selectedUnit );
        }
    }

    public void SelectUnit( int x, int y )
    {
        DeselectUnit( false );
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
            _tempUnitModel = _life.Units[ i ];
            _tempUnitModel.Props[ R.Altitude ].Value = _hexMapModel.Table[ _tempUnitModel.X ][ _tempUnitModel.Y ].Props[ R.Altitude ].Value; //not sure if this is needed? only if regenerating planets, but keeping units
            _tempUnitModel.Y = _tempUnitModel.Y; //update position vector3
            _hexMapModel.Table[ _tempUnitModel.X ][ _tempUnitModel.Y ].Unit = _tempUnitModel;
            _unitDefenseUpdateCommand.Execute( _tempUnitModel );
            _tempUnitModel.Setup();
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
        //update all units passive skills
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            _tempUnitModel = _life.Units[ i ];
            for( int j = 0; j < _tempUnitModel.PassiveSkills.Count; j++ )
            {
                _skillCommand.Execute( _tempUnitModel, _tempUnitModel.PassiveSkills[ j ] );
            }
        }
    }

}
