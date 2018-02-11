using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCommand : MonoBehaviour
{
    private static GameCommand _instance;
    public static GameCommand Instance { get { return _instance; } }

    private Dictionary<string, ICommand> _commands;
    
    // Use this for initialization
    void Awake()
    {
        _instance = this;
        _instance.Init();
    }

    private void Init()
    {
        _instance._commands = new Dictionary<string, ICommand>();
        Debug.Log( "GameCommand Awaken" );
    }

    public static void Execute<ICommand>( params object[] data )
    {
        string className = typeof( ICommand ).Name;
        if( _instance._commands.ContainsKey( className ) )
        {
            _instance._commands[ className ].Execute( data );
        }
    }

    public static void Register( ICommand command )
    {
        _instance._commands.Add( command.GetType().Name, command );
    }
}

public interface ICommand
{
    void Execute( params object[] data );
}
