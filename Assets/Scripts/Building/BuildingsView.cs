using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingsView : GameView
{
    public GameObject BuildingPrefab;
    private LifeModel _life;

    private Dictionary<BuildingModel, GameObject> _buildingsGO;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren();

        _buildingsGO = new Dictionary<BuildingModel, GameObject>();
        _life = value.Life;
        for( int i = 0; i < _life.Buildings.Count; i++ )
        {
            Add( _life.Buildings[ i ] );
        }
        disposables.Clear();
        _life.Buildings.ObserveAdd().Subscribe( _ => Add( _.Value ) ).AddTo( disposables );
        _life.Buildings.ObserveRemove().Subscribe( _ => Remove( _.Value ) ).AddTo( disposables );
    }

    private void Remove( BuildingModel model )
    {
        GameObject go = _buildingsGO[ model ];
        go.transform.SetParent( null );
        DestroyImmediate( go );
        _buildingsGO.Remove( model );
    }

    private void Add( BuildingModel model )
    {
        GameObject unitGO = Instantiate(
                    BuildingPrefab,
                    new Vector3( 0, 0, 0 ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        Building u = unitGO.GetComponent<Building>();
        u.SetModel( model );
        _buildingsGO.Add( model, unitGO );
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
