using System;
using System.Collections.Generic;

public class UnitController : AbstractController
{
    public UnitModel SelectedUnit { get { return _selectedUnit; } }

    private LifeModel _life;
    private GridModel<HexModel> _hexMapModel;

    private GridModel<UnitModel> _unitMap;
    private UnitModel _selectedUnit;

    private List<HexModel> _markedHexes;

    private UnitPaymentService _pay;
    private bool _isInAddMode;

    private RDictionary<double> _updateValues = new RDictionary<double>( true );

    public void Load( PlanetModel planet )
    {
        _hexMapModel = planet.Map;
        _life = planet.Life;
        _unitMap = new GridModel<UnitModel>( planet.Map.Width, planet.Map.Height );
        _markedHexes = new List<HexModel>();

        GameMessage.Listen<HexClickedMessage>( OnHexClickedMessage );
        GameMessage.Listen<UnitMessage>( OnUnitMessage );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );

        if( _pay == null )
            _pay = GameModel.Get<UnitPaymentService>();

        int x, y;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            x = _life.Units[ i ].X.Value;
            y = _life.Units[ i ].Y.Value;
            _unitMap.Table[ x, y ] = _life.Units[ i ];
            _life.Units[ i ].Altitude.Value = _hexMapModel.Table[ x, y ].Props[ R.Altitude ].Value;
        }
    }

    private void OnClockTick( ClockTickMessage value )
    {
        UpdateStep();
    }

    private void UpdateStep()
    {
        UnitModel um;
        HexModel hm;
        _updateValues.SetAll( 0 );

        for( int i = 0; i < _life.Units.Count; i++ )
        {
            um = _life.Units[ i ];
            hm = _hexMapModel.Table[ um.X.Value, um.Y.Value ];
            _updateValues[ R.Energy ] += hm.Props[ R.Energy].Value + um.AbilitiesDelta[ R.Energy ];
            _updateValues[ R.Science ] += hm.Props[ R.Science ].Value;
            _updateValues[ R.Minerals ] += hm.Props[ R.Minerals ].Value;

            hm.Props[ R.Temperature ].Value += um.AbilitiesDelta[ R.Temperature ];
            hm.Props[ R.Pressure ].Value += um.AbilitiesDelta[ R.Pressure ];
            hm.Props[ R.Humidity ].Value += um.AbilitiesDelta[ R.Humidity ];
            hm.Props[ R.Radiation ].Value += um.AbilitiesDelta[ R.Radiation ];

        }

        _life.Props[ R.Energy ].Value += _updateValues[ R.Energy ];
        _life.Props[ R.Energy ].Delta = _updateValues[ R.Energy ];

        _life.Props[ R.Science ].Value += _updateValues[ R.Science ];
        _life.Props[ R.Science ].Delta = _updateValues[ R.Science ];

        _life.Props[ R.Minerals ].Value += _updateValues[ R.Minerals ];
        _life.Props[ R.Minerals ].Delta = _updateValues[ R.Minerals ];
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
        if( _unitMap.Table[ x, y ] != null )
            return;

        UnitModel um = new UnitModel( x, y, _hexMapModel.Table[ x, y ].Props[ R.Altitude ].Value );
        _unitMap.Table[ x, y ] = um;
        _life.Units.Add( um );
        _life.Props[ R.Population ].Value++;
        SelectUnit( x, y );
    }

    private void MoveUnit( int xTo, int yTo )
    {
        _unitMap.Table[ _selectedUnit.X.Value, _selectedUnit.Y.Value ] = null;
        _unitMap.Table[ xTo, yTo ] = _selectedUnit;
        _selectedUnit.Altitude.Value = _hexMapModel.Table[ xTo, yTo ].Props[ R.Altitude].Value;
        _selectedUnit.X.Value = xTo;
        _selectedUnit.Y.Value = yTo;
    }

    private void OnHexClickedMessage( HexClickedMessage value )
    {
        int x = value.Hex.X;
        int y = value.Hex.Y;

        if( _isInAddMode )
        {
            if( _hexMapModel.Table[ x, y ].isMarked.Value == true )
            {
                if( _pay.BuyAddUnit() )
                {
                    AddUnit( x, y );
                    _isInAddMode = false;
                }
            }
            return;
        }

        if( _selectedUnit != null && _hexMapModel.Table[ x, y ].isMarked.Value == true )
        {
            if( _pay.BuyMoveUnit() )
            {
                MoveUnit( x, y );
                SelectUnit( x, y );
            }
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
        UnmarkHexes();
        if( _selectedUnit != null )
        {
            _selectedUnit.isSelected.Value = false;
            _selectedUnit = null;
            GameModel.Set( _selectedUnit );
        }
    }

    private void SelectUnit( int x, int y )
    {
        DeselectUnit();
        _selectedUnit = _unitMap.Table[ x, y ];
        _selectedUnit.isSelected.Value = true;
        _hexMapModel.Table[ x, y ].isExplored.Value = true;
        MarkNeighborHexes( _selectedUnit );
        GameModel.Set( _selectedUnit );
    }

    private void MarkNeighborHexes( UnitModel unit )
    {
        int x = unit.X.Value;
        int y = unit.Y.Value;

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
            _hexMapModel.Table[ x, y ].isExplored.Value = true;
            if( _unitMap.Table[ x, y ] == null &&
                Math.Abs( _hexMapModel.Table[ x, y ].Props[ R.Altitude ].Value - unit.Altitude.Value ) <= _life.ClimbLevel ) //check if it can climb
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
