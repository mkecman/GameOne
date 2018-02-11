using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;

public class GameModel : MonoBehaviour
{
    private static GameModel _instance;
    public static GameModel Instance { get { return _instance; } }
    public delegate void ModelDelegate<T>( T value );

    private Dictionary<string, object> _models;
    private Dictionary<string, object> _binding;
    
    public static void Set<T>( T value )
    {
        string className = typeof( T ).Name;

        if( !_instance._models.ContainsKey( className ) )
            _instance._models.Add( className, value );
        else
            _instance._models[ className ] = value;

        if( _instance._binding.ContainsKey( className ) )
        {
            ModelDelegate<T> modelDelegate = (ModelDelegate<T>)_instance._binding[ className ];
            modelDelegate( value );
        }
    }
    
    public static void HandleGet<T>( ModelDelegate<T> handler )
    {
        string className = typeof( T ).Name;
        ModelDelegate<T> modelDelegate;

        if( !_instance._binding.ContainsKey( className ) )
        {
            modelDelegate = handler;
            _instance._binding.Add( className, modelDelegate );
        }
        else
        {
            modelDelegate = (ModelDelegate<T>)_instance._binding[ className ];
            modelDelegate += handler;
            _instance._binding[ className ] = modelDelegate;
        }

        if( _instance._models.ContainsKey( className ) )
            handler( (T)_instance._models[ className ] );
    }

    public static T Get<T>()
    {
        string className = typeof( T ).Name;
        if( _instance._models.ContainsKey( className ) )
            return (T)_instance._models[ className ];

        return default(T);
    }

    public static void Remove<T>( ModelDelegate<T> handler )
    {
        string className = typeof( T ).Name;

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
        return JsonMapper.ToObject<T>( JsonMapper.ToJson( data ) );
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
        _instance._models = new Dictionary<string, object>();
        _instance._binding = new Dictionary<string, object>();
        Debug.Log( "GameModel Awaken" );
    }
}
