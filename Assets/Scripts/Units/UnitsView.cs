using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UnitsView : GameView
{
    public GameObject UnitPrefab;
    private LifeModel _life;

    private Dictionary<UnitModel, GameObject> _unitsGO;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren( gameObject.transform );

        _unitsGO = new Dictionary<UnitModel, GameObject>();
        _life = value.Life;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            AddUnit( _life.Units[ i ] );
        }
        disposables.Clear();
        _life.Units.ObserveAdd().Subscribe( _ => AddUnit( _.Value ) ).AddTo( disposables );
        _life.Units.ObserveRemove().Subscribe( _ => RemoveUnit( _.Value ) ).AddTo( disposables );
    }

    private void RemoveUnit( UnitModel value )
    {
        GameObject go = _unitsGO[ value ];
        go.transform.SetParent( null );
        DestroyImmediate( go );
        _unitsGO.Remove( value );
    }

    private void AddUnit( UnitModel unit )
    {
        GameObject unitGO = Instantiate(
                    UnitPrefab,
                    new Vector3( 0,0,0 ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        Unit u = unitGO.GetComponent<Unit>();
        u.SetModel( unit );
        _unitsGO.Add( unit, unitGO );
    }
    
}
