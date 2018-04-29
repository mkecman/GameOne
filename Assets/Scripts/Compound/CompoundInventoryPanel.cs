using UnityEngine;
using System.Collections;
using System;
using UniRx;
using System.Collections.Generic;

public class CompoundInventoryPanel : GameView
{
    public GameObject CompoundInventoryPrefab;
    private ReactiveDictionary<int, IntReactiveProperty> _LifeCompounds;
    private CompoundConfig _compounds;

    private void Start()
    {
        _compounds = GameConfig.Get<CompoundConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        disposables.Clear();
        RemoveAllChildren( transform );

        _LifeCompounds = value.Life.Compounds;

        foreach( KeyValuePair<int, IntReactiveProperty> item in _LifeCompounds )
        {
            Add( _compounds[ item.Key ], item.Value );
        }

        _LifeCompounds.ObserveAdd().Subscribe( _ => Add( _compounds[ _.Key ], _.Value ) ).AddTo( disposables );
    }

    private void Add( CompoundJSON compoundJSON, IntReactiveProperty value )
    {
        Instantiate( CompoundInventoryPrefab, transform )
            .GetComponent<CompoundInventoryView>()
            .Setup( compoundJSON, value );
    }

}
