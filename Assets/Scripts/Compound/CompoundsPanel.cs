using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UniRx;

public class CompoundsPanel : GameView
{
    public Transform Container;
    public GameObject CompoundUnlockPrefab;
    public GameObject EffectPrefab;
    private CompoundConfig _compoundConfig;
    private CompoundJSON _compound;

    // Use this for initialization
    void Awake()
    {
        _compoundConfig = GameConfig.Get<CompoundConfig>();
    }

    public void SetModel( LifeModel life, CompoundType type )
    {
        RemoveAllChildren( Container );
        for( int i = 0; i < _compoundConfig.Count; i++ )
        {
            _compound = _compoundConfig[ i ];
            if( _compound.Type == type )
            {
                GameObject go = Instantiate( CompoundUnlockPrefab, Container );
                go.GetComponent<CompoundView>().Setup( _compound, EffectPrefab );
            }
        }
    }

}
