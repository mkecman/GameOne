using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UnitsView : GameView
{
    public GameObject UnitPrefab;
    private LifeModel _life;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren();
        _life = value.Life;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            AddUnit( _life.Units[ i ] );
        }
        disposables.Clear();
        _life.Units.ObserveAdd().Subscribe( _ => AddUnit( _.Value ) ).AddTo( disposables );
    }
    
    private void AddUnit( UnitModel unit )
    {
        int x = unit.X.Value;
        int y = unit.Y.Value;

        GameObject unitGO = (GameObject)Instantiate(
                    UnitPrefab,
                    new Vector3( HexMapHelper.GetXPosition( x, y ), (float)unit.Altitude.Value, HexMapHelper.GetZPosition( y ) ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        Unit u = unitGO.GetComponent<Unit>();
        u.SetModel( unit );
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
