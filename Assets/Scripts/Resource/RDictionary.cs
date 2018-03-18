﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class RDictionary<T>
{
    public int Count
    {
        get
        {
            return Values.Count;
        }
    }

    public T this[ R key ]
    {
        get
        {
            //if( !Values.ContainsKey( key ) )
            //    Values.Add( key, default( T ) );

            return Values[ key ];
        }
        set
        {
            Values[ key ] = value;
        }
    }

    private Dictionary<R, T> Values;

    public RDictionary( bool setDefaulValues = false )
    {
        Values = new Dictionary<R, T>();
        if( setDefaulValues )
        {
            for( int i = 0; i < (int)R.Count; i++ )
            {
                Values.Add( (R)i, getConstructor<T>()() );
            }
        }
    }

    public void Add( R type, T value )
    {
        Values.Add( type, value );
    }

    public void SetAll( T value )
    {
        Values = Values.ToDictionary( p => p.Key, p => value );
    }

    private Func<T> getConstructor<T>()
    {
        System.Reflection.ConstructorInfo constructor = ( typeof( T ) ).GetConstructor( System.Type.EmptyTypes );
        if( ReferenceEquals( constructor, null ) )
        {
            return () => { return default( T ); };
        }
        else
        {
            return () => { return (T)constructor.Invoke( new object[ 0 ] ); };
        }
    }

}
