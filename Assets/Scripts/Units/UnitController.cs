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

    private List<HexModel> _markedHexes;

    private UnitPaymentService _pay;
    private bool _isInAddMode;
    private bool _isAddingUnit;

    private Dictionary<R, float> _updateValues = new Dictionary<R, float>();
    private HexUpdateCommand _hexUpdateCommand;
    private GameDebug _debug;
    private Dictionary<R, BellCurve> _bellCurves;

    public void Init()
    {
        _updateValues.Add( R.Energy, 0 );
        _updateValues.Add( R.Science, 0 );
        _updateValues.Add( R.Minerals, 0 );

        _pay = GameModel.Get<UnitPaymentService>();
        _hexUpdateCommand = GameModel.Get<HexUpdateCommand>();
        _debug = GameModel.Get<GameDebug>();
        _bellCurves = GameConfig.Get<BellCurveConfig>();

        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _hexMapModel = value.Map;
        _life = value.Life;
        _markedHexes = new List<HexModel>();

        GameMessage.Listen<HexClickedMessage>( OnHexClickedMessage );
        GameMessage.Listen<UnitMessage>( OnUnitMessage );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
        GameMessage.Listen<ResistanceUpgradeMessage>( OnResistanceUpgrade );

        UnitModel um;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            um = _life.Units[ i ];
            um.Props[ R.Altitude ].Value = _hexMapModel.Table[ um.X ][ um.Y ].Props[ R.Altitude ].Value; //not sure if this is needed? only if regenerating planets, but keeping units
            um.Y = um.Y; //update position vector3
            _hexMapModel.Table[ um.X ][ um.Y ].Unit = um;
        }

        //_life.Props[ R.Energy ]._Value.Where( _ => _ > _pay.GetAddUnitPrice() && !_isAddingUnit ).Subscribe( _ => AddRandomUnit() );
    }

    private void AddRandomUnit()
    {
        DeselectUnit();
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            MarkNeighborHexes( _life.Units[ i ] );
        }
        HexModel _hex;
        float max = 0;
        int x = 0, y = 0;
        for( int i = 0; i < _markedHexes.Count; i++ )
        {
            _hex = _markedHexes[ i ];
            if( _hex.Props[ R.Energy ].Value > max ) //find highest food output
            {
                max = _hex.Props[ R.Energy ].Value;
                x = _hex.X;
                y = _hex.Y;
            }
        }

        _isAddingUnit = true;
        if( _pay.BuyAddUnit() )
            AddUnit( x, y );
        _isAddingUnit = false;
    }

    private void OnResistanceUpgrade( ResistanceUpgradeMessage value )
    {
        float delta = Mathf.Abs( value.Delta );
        if( _selectedUnit.Resistance[ value.Type ].ChangePosition( value.Delta ) )
            delta = -delta;

        _selectedUnit.AbilitiesDelta[ R.Science ].Value = _selectedUnit.AbilitiesDelta[ R.Science ].Value.Sum( delta );

        UpdateHexAndHealth( _selectedUnit.Resistance, _hexMapModel.Table[ _selectedUnit.X ][ _selectedUnit.Y ] );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        UpdateStep();
    }

    private void UpdateStep()
    {
        UnitModel um;
        HexModel hm;
        _updateValues[ R.Energy ] = 0;
        _updateValues[ R.Science ] = 0;
        _updateValues[ R.Minerals ] = 0;

        for( int i = 0; i < _life.Units.Count; i++ )
        {
            um = _life.Units[ i ];
            hm = _hexMapModel.Table[ um.X ][ um.Y ];

            /*
            um.Props[ R.Health ].Value -= 1 - hm.Props[ R.HexScore ].Value;
            if( um.Props[ R.Health ].Value <= 0 )
            {
                RemoveUnit( um );
                continue;
            }
            */
            //Moved to when a unit is moved onto a new hexmodel
            //um.Props[ R.Health ].Value = hm.Props[ R.HexScore ].Value;

            _updateValues[ R.Energy ] += ( hm.Props[ R.Energy ].Value * um.Props[ R.Health ].Value ) + um.AbilitiesDelta[ R.Energy ].Value;
            _updateValues[ R.Science ] += ( hm.Props[ R.Science ].Value * um.Props[ R.Health ].Value ) + um.AbilitiesDelta[ R.Science ].Value;
            _updateValues[ R.Minerals ] += ( hm.Props[ R.Minerals ].Value * um.Props[ R.Health ].Value ) + um.AbilitiesDelta[ R.Minerals ].Value;

            /* Moved to BuildingController
            hm.Props[ R.Temperature ].Value += um.AbilitiesDelta[ R.Temperature ].Value;
            hm.Props[ R.Pressure ].Value += um.AbilitiesDelta[ R.Pressure ].Value;
            hm.Props[ R.Humidity ].Value += um.AbilitiesDelta[ R.Humidity ].Value;
            hm.Props[ R.Radiation ].Value += um.AbilitiesDelta[ R.Radiation ].Value;
            _hexUpdateCommand.Execute( um.Resistance, hm );
            */

        }

        _life.Props[ R.Energy ].Value += _updateValues[ R.Energy ];
        //_life.Props[ R.Energy ].Delta = _updateValues[ R.Energy ];

        _life.Props[ R.Science ].Value += _updateValues[ R.Science ];
        //_life.Props[ R.Science ].Delta = _updateValues[ R.Science ];

        _life.Props[ R.Minerals ].Value += _updateValues[ R.Minerals ];
        //_life.Props[ R.Minerals ].Delta = _updateValues[ R.Minerals ];
    }

    private void OnUnitMessage( UnitMessage value )
    {
        if( value.Type == UnitMessageType.Add )
        {
            if( _isInAddMode )
            {
                _isInAddMode = false;
                DeselectUnit();
            }
            else
            if( _pay.BuyAddUnit( false ) )
            {
                //AddUnit( value.X, value.Y );
                DeselectUnit();
                for( int i = 0; i < _life.Units.Count; i++ )
                {
                    MarkNeighborHexes( _life.Units[ i ] );
                }
                _isInAddMode = true;
            }
        }
        else
        {

        }
    }

    private void AddUnit( int x, int y )
    {
        if( _hexMapModel.Table[ x ][ y ].Unit != null )
            return;

        UnitModel um = new UnitModel( x, y, _hexMapModel.Table[ x ][ y ].Props[ R.Altitude ].Value, GameModel.Copy( _bellCurves ) );
        _hexMapModel.Table[ x ][ y ].Unit = um;
        _life.Units.Add( um );
        _life.Props[ R.Population ].Value++;
        SelectUnit( x, y );
        UpdateHexAndHealth( _selectedUnit.Resistance, _hexMapModel.Table[ x ][ y ] );
    }

    private void RemoveUnit( UnitModel um )
    {
        if( _selectedUnit == um )
            DeselectUnit();

        _life.Units.Remove( um );
        _hexMapModel.Table[ um.X ][ um.Y ].Unit = null;
        _life.Props[ R.Population ].Value--;
    }

    private void MoveUnit( int xTo, int yTo )
    {
        HexModel hexModel = _hexMapModel.Table[ _selectedUnit.X ][ _selectedUnit.Y ];
        if( hexModel.Building != null )
            hexModel.Building.State = BuildingState.INACTIVE;

        hexModel.Unit = null;
        _hexUpdateCommand.Execute( _life.Resistance, hexModel );

        _hexMapModel.Table[ xTo ][ yTo ].Unit = _selectedUnit;

        _selectedUnit.Props[ R.Altitude ].Value = _hexMapModel.Table[ xTo ][ yTo ].Props[ R.Altitude ].Value;
        _selectedUnit.X = xTo;
        _selectedUnit.Y = yTo;

        UpdateHexAndHealth( _selectedUnit.Resistance, _hexMapModel.Table[ xTo ][ yTo ] );
    }

    private void UpdateHexAndHealth( Dictionary<R, BellCurve> resistance, HexModel hexModel )
    {
        _hexUpdateCommand.Execute( resistance, hexModel );
        _selectedUnit.Props[ R.Health ].Value = hexModel.Props[ R.HexScore ].Value;
    }

    private void OnHexClickedMessage( HexClickedMessage value )
    {
        int x = value.Hex.X;
        int y = value.Hex.Y;

        if( _isInAddMode )
        {
            if( _hexMapModel.Table[ x ][ y ].isMarked.Value == true )
            {
                if( _pay.BuyAddUnit() )
                {
                    AddUnit( x, y );
                    _isInAddMode = false;
                }
            }
            return;
        }

        if( _selectedUnit != null && _hexMapModel.Table[ x ][ y ].isMarked.Value == true )
        {
            if( _pay.BuyMoveUnit() )
            {
                MoveUnit( x, y );
                SelectUnit( x, y );
            }
        }

        //Unit is in the clicked tile
        if( _hexMapModel.Table[ x ][ y ].Unit != null )
        {
            SelectUnit( x, y );
        }
        else
        {
            DeselectUnit();
        }

        GameModel.Set<HexModel>( value.Hex );
    }

    private void DeselectUnit()
    {
        UnmarkHexes();
        if( _selectedUnit != null )
        {
            _selectedUnit.isSelected.Value = false;
            _selectedUnit = null;
            GameModel.Set<UnitModel>( _selectedUnit );
        }
    }

    private void SelectUnit( int x, int y )
    {
        DeselectUnit();
        _selectedUnit = _hexMapModel.Table[ x ][ y ].Unit;
        _selectedUnit.isSelected.Value = true;
        _hexMapModel.Table[ x ][ y ].isExplored.Value = true;
        MarkNeighborHexes( _selectedUnit );
        GameModel.Set<UnitModel>( _selectedUnit );
    }

    private void MarkNeighborHexes( UnitModel unit )
    {
        int x = unit.X;
        int y = unit.Y;

        CheckAndMark( x, y + 1, unit );

        CheckAndMark( x - 1, y, unit );
        CheckAndMark( x, y, unit );
        CheckAndMark( x + 1, y, unit );

        CheckAndMark( x, y - 1, unit );

        if( y % 2 == 0 )
        {
            CheckAndMark( x - 1, y + 1, unit );
            CheckAndMark( x - 1, y - 1, unit );
        }
        else
        {
            CheckAndMark( x + 1, y + 1, unit );
            CheckAndMark( x + 1, y - 1, unit );
        }
    }

    private void CheckAndMark( int x, int y, UnitModel unit )
    {
        if( x >= 0 && y >= 0 && x < _hexMapModel.Width && y < _hexMapModel.Height )
        {
            _hexMapModel.Table[ x ][ y ].isExplored.Value = true;
            if( _hexMapModel.Table[ x ][ y ].Unit == null &&
                Math.Abs( _hexMapModel.Table[ x ][ y ].Props[ R.Altitude ].Value - unit.Props[ R.Altitude ].Value ) <= _life.ClimbLevel ) //check if it can climb
            {
                _hexMapModel.Table[ x ][ y ].isMarked.Value = true;
                _markedHexes.Add( _hexMapModel.Table[ x ][ y ] );
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
