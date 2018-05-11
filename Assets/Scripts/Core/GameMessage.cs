using System;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class GameMessage : MonoBehaviour
{
    public delegate void MessageDelegate<T>( T value );

    private static GameMessage _instance;
    private Dictionary<Type, object> _messages;
    private static Type _className;

    public static void Listen<T>( MessageDelegate<T> handler )
    {
        _className = typeof( T );

        if( !_instance._messages.ContainsKey( _className ) )
        {
            _instance._messages.Add( _className, handler );
        }
        else
        {
            MessageDelegate<T> messageDelegate;
            messageDelegate = (MessageDelegate<T>)_instance._messages[ _className ];
            messageDelegate -= handler; //remove handler in case it's already added
            messageDelegate += handler;
            _instance._messages[ _className ] = messageDelegate;
        }
    }

    public static void StopListen<T>( MessageDelegate<T> handler )
    {
        _className = typeof( T );

        if( _instance._messages.ContainsKey( _className ) )
        {
            MessageDelegate<T> messageDelegate = (MessageDelegate<T>)_instance._messages[ _className ];
            messageDelegate -= handler;

            if( messageDelegate == null )
                _instance._messages.Remove( _className );
            else
                _instance._messages[ _className ] = messageDelegate;
        }
    }

    public static void Send<T>( T message )
    {
        _className = typeof( T );
        if( _instance._messages.ContainsKey( _className ) )
        {
            ( _instance._messages[ _className ] as MessageDelegate<T> ).Invoke( message );
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
        _instance._messages = new Dictionary<Type, object>();
        //Debug.Log( "GameMessage Init" );
    }
}
