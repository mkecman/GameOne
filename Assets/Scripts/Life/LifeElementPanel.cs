using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;
using System;

public class LifeElementPanel : GameView
{
    public GameObject Prefab;
    private Dictionary<LifeElementModel, GameObject> _children;
    private LifeModel _life;

    // Use this for initialization
    void Start()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        RemoveAllChildren( gameObject.transform );

        _children = new Dictionary<LifeElementModel, GameObject>();
        _life = value.Life;
        foreach( KeyValuePair<int, LifeElementModel> item in _life.Elements )
        {
            Add( item.Value );
        }
        disposables.Clear();
        _life.Elements.ObserveAdd().Subscribe( _ => Add( _.Value ) ).AddTo( disposables );
        _life.Elements.ObserveRemove().Subscribe( _ => Remove( _.Value ) ).AddTo( disposables );
    }

    private void Remove( LifeElementModel value )
    {
        GameObject go = _children[ value ];
        go.transform.SetParent( null );
        DestroyImmediate( go );
        _children.Remove( value );
    }

    private void Add( LifeElementModel value )
    {
        GameObject unitGO = Instantiate(
                    Prefab,
                    new Vector3( 0, 0, 0 ),
                    Quaternion.identity );
        unitGO.transform.SetParent( this.transform );
        unitGO.GetComponent<LifeElementView>().SetModel( value );
        _children.Add( value, unitGO );

        SetOrder();
    }

    private void SetOrder()
    {
        List<LifeElementModel> sorted = _children.Keys.ToList();
        sorted.Sort( SortChildrenKeys );

        for( int i = 0; i < sorted.Count; i++ )
            _children[ sorted[ i ] ].transform.SetSiblingIndex( i );
    }

    private int SortChildrenKeys( LifeElementModel x, LifeElementModel y )
    {
        if( x.Index > y.Index )
            return 1;
        if( x.Index < y.Index )
            return -1;

        return 0;
    }
}
