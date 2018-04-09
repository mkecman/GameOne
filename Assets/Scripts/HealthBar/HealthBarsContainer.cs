using UnityEngine;
using UniRx;
using System.Collections.Generic;

public class HealthBarsContainer : GameView
{
    public GameObject Prefab;
    private LifeModel _life;

    private Dictionary<UnitModel, GameObject> _children;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren( gameObject.transform );

        _children = new Dictionary<UnitModel, GameObject>();
        _life = value.Life;
        for( int i = 0; i < _life.Units.Count; i++ )
        {
            Add( _life.Units[ i ] );
        }
        disposables.Clear();
        _life.Units.ObserveAdd().Subscribe( _ => Add( _.Value ) ).AddTo( disposables );
        _life.Units.ObserveRemove().Subscribe( _ => Remove( _.Value ) ).AddTo( disposables );
    }

    private void Remove( UnitModel value )
    {
        GameObject go = _children[ value ];
        go.transform.SetParent( null );
        DestroyImmediate( go );
        _children.Remove( value );
    }

    private void Add( UnitModel unit )
    {
        GameObject unitGO = Instantiate(
                    Prefab,
                    new Vector3( 0,0,0 ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        unitGO.GetComponent<HealthBar>().SetModel( unit );
        _children.Add( unit, unitGO );
    }
    
}
