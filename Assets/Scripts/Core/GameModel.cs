using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    private static GameModel _instance;
    public static GameModel Instance { get { return _instance; } }
    public delegate void ModelDelegate<T>( T value );

    private Dictionary<Type, object> _models;
    private Dictionary<Type, object> _binding;
    private static Type className;

    public static void Set<T>( T value )
    {
        className = typeof( T );

        if( !_instance._models.ContainsKey( className ) )
            _instance._models.Add( className, value );
        else
            _instance._models[ className ] = value;

        if( _instance._binding.ContainsKey( className ) )
            ( _instance._binding[ className ] as ModelDelegate<T> ).Invoke( value );
    }

    public static void HandleGet<T>( ModelDelegate<T> handler )
    {
        className = typeof( T );

        if( !_instance._binding.ContainsKey( className ) )
        {
            _instance._binding.Add( className, handler );
        }
        else
        {
            ModelDelegate<T> modelDelegate = (ModelDelegate<T>)_instance._binding[ className ];
            modelDelegate += handler;
            _instance._binding[ className ] = modelDelegate;
        }

        if( _instance._models.ContainsKey( className ) )
            handler( (T)_instance._models[ className ] );
    }

    public static T Get<T>()
    {
        className = typeof( T );
        if( _instance._models.ContainsKey( className ) )
            return (T)_instance._models[ className ];

        throw new Exception( "Class " + className.Name + " is not present as GameModel." );
    }

    public static void RemoveHandle<T>( ModelDelegate<T> handler )
    {
        className = typeof( T );

        if( _instance._binding.ContainsKey( className ) )
        {
            ModelDelegate<T> modelDelegate = (ModelDelegate<T>)_instance._binding[ className ];
            modelDelegate -= handler;

            if( modelDelegate == null )
                _instance._binding.Remove( className );
            else
                _instance._binding[ className ] = modelDelegate;
        }

    }

    public static T Copy<T>( T data )
    {
        return JsonConvert.DeserializeObject<T>( JsonConvert.SerializeObject( data ) );
    }

    // Use this for initialization
    void Awake()
    {
        if( _instance != null && _instance != this )
        {
            //Destroy( this.gameObject );
        }
        else
        {
            _instance = this;
        }
        _instance = this;
        _instance.Init();
    }

    private void Init()
    {
        _instance._models = new Dictionary<Type, object>();
        _instance._binding = new Dictionary<Type, object>();
        //Debug.Log( "GameModel Awaken" );
    }
}
