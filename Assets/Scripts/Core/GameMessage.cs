using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class GameMessage : MonoBehaviour
{
    public delegate void MessageDelegate<T>( T value );

    private static GameMessage _instance;
    private Dictionary<string, object> _messages;
    
    public static void Listen<T>( MessageDelegate<T> handler )
    {
        string className = typeof( T ).Name;
        MessageDelegate<T> messageDelegate;

        if( !_instance._messages.ContainsKey( className ) )
        {
            messageDelegate = handler;
            _instance._messages.Add( className, messageDelegate );
        }
        else
        {
            messageDelegate = (MessageDelegate<T>)_instance._messages[ className ];
            messageDelegate -= handler; //remove handler in case it's already added
            messageDelegate += handler;
            _instance._messages[ className ] = messageDelegate;
        }
    }
    
    public static void StopListen<T>( MessageDelegate<T> handler )
    {
        string className = typeof( T ).Name;

        if( _instance._messages.ContainsKey( className ) )
        {
            MessageDelegate<T> messageDelegate = (MessageDelegate<T>)_instance._messages[ className ];
            messageDelegate -= handler;

            if( messageDelegate == null )
                _instance._messages.Remove( className );
            else
                _instance._messages[ className ] = messageDelegate;
        }
    }

    public static void Send<T>( T message )
    {
        string className = typeof( T ).Name;
        if( _instance._messages.ContainsKey( className ) )
        {
            MessageDelegate<T> messageDelegate = (MessageDelegate<T>)_instance._messages[ className ];
            messageDelegate( message );
        }
    }

    // Use this for initialization
    void Awake()
    {
        /*
        if( _instance != null && _instance != this )
        {
            //Destroy( this.gameObject );
        }
        else
        {
            _instance = this;
        }
        */
        _instance = this;
        _instance.Init();
    }
    
    private void Init()
    {
        _instance._messages = new Dictionary<string, object>();
        Debug.Log( "GameMessage Init" );
    }
}
