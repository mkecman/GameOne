using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnitModelTest : MonoBehaviour
{
    private List<UnitModel> _units;
    private UnitFactory _factory;

    // Use this for initialization
    void Start()
    {
        Invoke( "RealStart", 3 );
        
    }

    public void RealStart()
    {
        Debug.Log( "RealStart" );
        _units = new List<UnitModel>();
        _factory = GameModel.Get<UnitFactory>();

        for( int i = 0; i < 1000; i++ )
        {
            _units.Add( _factory.GetUnit( 0, 0 ) );
        }

        Invoke( "Dispose", 2 );
    }

    public void Dispose()
    {
        Debug.Log( "Dispose" );
        for( int i = 0; i < 1000; i++ )
        {
            _units[ i ].Dispose();
        }
        _units.Clear();
        _units = null;
        GC.Collect();
    }
}
