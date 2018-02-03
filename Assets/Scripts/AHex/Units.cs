using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Units : MonoBehaviour
{
    public GameObject UnitPrefab;

    private List<UnitModel> _units;
    private HexConfig _hexConfig;
    private GridModel<GameObject> _mapModel;

    private HexMapModel _hexMapModel;

    private GameObject _selectedUnit;

    // Use this for initialization
    void Start()
    {
        _units = new List<UnitModel>();
        _hexConfig = Config.Get<HexConfig>();
        GameModel.Bind<HexMapModel>( OnHexMapModelChange );
        GameMessage.Listen<UnitMoveMessage>( OnMoveUnitMessage );
    }

    private void OnMoveUnitMessage( UnitMoveMessage value )
    {
        MoveUnit( value.X, value.Y );
    }

    private void OnHexMapModelChange( HexMapModel value )
    {
        RemoveAllChildren();
        _hexMapModel = value;
        _mapModel = new GridModel<GameObject>( _hexMapModel.Width, _hexMapModel.Height );
        //SetStartingPosition( Mathf.RoundToInt( _hexMapModel.Width / 2 ), Mathf.RoundToInt( _hexMapModel.Height / 2 ) );
        AddUnit( 29, 29 );
    }

    public void SetStartingPosition( int x, int y )
    {
        Debug.Log( "Adding unit: " + x + "," + y );
        AddUnit( x, y );
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AddUnit( int x, int y )
    {
        float xPos = x * _hexConfig.xOffset;
        if( y % 2 == 1 )
            xPos += _hexConfig.xOffset / 2f;

        GameObject unitGO = (GameObject)Instantiate(
                    UnitPrefab,
                    new Vector3( xPos, _hexMapModel.heightMap.Table[x,y], y * _hexConfig.zOffset ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        _mapModel.Table[ x, y ] = unitGO;
        _selectedUnit = unitGO;
    }

    private void MoveUnit( int xTo, int yTo )
    {
        float xPos = xTo * _hexConfig.xOffset;
        if( yTo % 2 == 1 )
            xPos += _hexConfig.xOffset / 2f;
        _selectedUnit.transform.position = new Vector3( xPos, _hexMapModel.heightMap.Table[ xTo, yTo ], yTo * _hexConfig.zOffset );
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
