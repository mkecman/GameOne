using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pool<T> : IDisposable where T : IDisposable
{
    //public int Count { get { return _items.Count; } }

    private Queue<T> _items;

    public Pool( int capacity = int.MaxValue )
    {
        if( capacity != int.MaxValue )
            _items = new Queue<T>( capacity );
        else
            _items = new Queue<T>();
    }

    public void Dispose()
    {
        while( _items.Count > 0 )
        {
            _items.Dequeue().Dispose();
        }
        _items = null;
    }

    public T Pull()
    {
        return ( _items.Count > 0 ) ? _items.Dequeue() : (T)Activator.CreateInstance(typeof(T));
    }

    public void Push( T item )
    {
        _items.Enqueue( item );
    }

    //protected abstract T CreateInstance();
}
