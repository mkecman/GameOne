using UnityEngine;
using System.Collections.Generic;

public class RDictionary<T>
{
    public T this[ R key ]
    {
        get
        {
            return Values[ key ];
        }
        set
        {
            Values[ key ] = value;
        }
    }

    private Dictionary<R, T> Values;

    public RDictionary()
    {
        Values = new Dictionary<R, T>();
        for( int i = 0; i < (int)R.Count; i++ )
        {
            Values.Add( (R)i, default( T ) );
        }
    }

    public void Set( R key, T value )
    {
        Values[ key ] = value;
    }
}
