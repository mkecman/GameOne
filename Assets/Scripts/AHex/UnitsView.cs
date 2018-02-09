﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UnitsView : MonoBehaviour
{
    public GameObject UnitPrefab;
    private LifeModel _life;

    // Use this for initialization
    void Start()
    {
        GameModel.Bind<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren();
        _life = value.Life;
        _life.Units.ObserveAdd().Subscribe( _ => AddUnit( _.Value ) ).AddTo( this );
    }
    
    private void AddUnit( UnitModel unit )
    {
        int x = unit.X.Value;
        int y = unit.Y.Value;

        GameObject unitGO = (GameObject)Instantiate(
                    UnitPrefab,
                    new Vector3( HexMapHelper.GetXPosition( x, y ), unit.Altitude.Value, HexMapHelper.GetZPosition( y ) ),
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
