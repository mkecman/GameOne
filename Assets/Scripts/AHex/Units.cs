using System;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public GameObject UnitPrefab;

    private List<UnitModel> _units;
    private GridModel<Unit> _unitMap;

    private HexMapModel _hexMapModel;
    private List<HexModel> _markedHexes;

    private LifeModel _life;

    private Unit _selectedUnit;

    private void Update()
    {
        if( Input.GetKeyDown( KeyCode.Space ) )
            AddUnit( Mathf.RoundToInt( _hexMapModel.Width / 2 ), Mathf.RoundToInt( _hexMapModel.Height / 2 ) );
    }

    // Use this for initialization
    void Start()
    {
        _units = new List<UnitModel>();
        _markedHexes = new List<HexModel>();
        GameModel.Bind<HexMapModel>( OnHexMapModelChange );
        GameModel.Bind<LifeModel>( OnLifeModelChange );
        GameMessage.Listen<HexClickedMessage>( OnHexClickedMessage );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
    }

    private void OnLifeModelChange( LifeModel value )
    {
        _life = value;
    }

    private void OnClockTick( ClockTickMessage value )
    {
        double food = 0;
        double science = 0;
        double words = 0;

        UnitModel um;
        ElementModel em;
        for( int i = 0; i < _units.Count; i++ )
        {
            um = _units[ i ];
            em = _hexMapModel.ElementMap.Table[ um.X, um.Y ];
            food += em.Modifier( ElementModifiers.Food ).Delta;
            science += em.Modifier( ElementModifiers.Science ).Delta;
            words += em.Modifier( ElementModifiers.Words ).Delta;
        }

        _life.Food += food;
        _life.Science += science;
        _life.Words += words;
    }
    
    private void OnHexClickedMessage( HexClickedMessage value )
    {
        int x = value.Hex.X;
        int y = value.Hex.Y;

        if( _selectedUnit != null && _hexMapModel.HexMap.Table[x,y].isMarked.Value == true )
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
            _selectedUnit.Model.isSelected.Value = false;
        }
    }

    private void SelectUnit( int x, int y )
    {
        DeselectUnit();
        _selectedUnit = _unitMap.Table[ x, y ];
        _selectedUnit.Model.isSelected.Value = true;
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
                _hexMapModel.HexMap.Table[ x, y ].isMarked.Value = true;
                _markedHexes.Add( _hexMapModel.HexMap.Table[ x, y ] );
            }
        }
    }

    private void UnmarkHexes()
    {
        for( int i = 0; i < _markedHexes.Count; i++ )
            _markedHexes[ i ].isMarked.Value = false;

        _markedHexes.Clear();
    }

    private void OnHexMapModelChange( HexMapModel value )
    {
        RemoveAllChildren();
        _hexMapModel = value;
        _unitMap = new GridModel<Unit>( _hexMapModel.Width, _hexMapModel.Height );
        AddUnit( Mathf.RoundToInt( _hexMapModel.Width / 2 ), Mathf.RoundToInt( _hexMapModel.Height / 2 ) );
    }
    
    private void AddUnit( int x, int y )
    {
        if( _unitMap.Table[ x, y ] != null )
            return;

        GameObject unitGO = (GameObject)Instantiate(
                    UnitPrefab,
                    new Vector3( HexMapHelper.GetXPosition( x, y ), _hexMapModel.AltitudeMap.Table[ x, y ], HexMapHelper.GetZPosition( y ) ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        UnitModel um = new UnitModel( x, y );
        Unit u = unitGO.GetComponent<Unit>();
        u.SetModel( um );

        _unitMap.Table[ x, y ] = u;
        _units.Add( um );
    }

    private void MoveUnit( int xTo, int yTo )
    {
        _unitMap.Table[ _selectedUnit.Model.X, _selectedUnit.Model.Y ] = null;
        _unitMap.Table[ xTo, yTo ] = _selectedUnit;
        _selectedUnit.Model.X = xTo;
        _selectedUnit.Model.Y = yTo;
        _selectedUnit.transform.position = new Vector3( HexMapHelper.GetXPosition( xTo, yTo ), _hexMapModel.AltitudeMap.Table[ xTo, yTo ], HexMapHelper.GetZPosition( yTo ) );
    }

    private void RemoveAllChildren()
    {
        GameObject go;
        while( gameObject.transform.childCount != 0 )
        {
            go = gameObject.transform.GetChild( 0 ).gameObject;
            go.transform.SetParent( null );
            DestroyImmediate( go );
        }
    }
}
