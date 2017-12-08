using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    private static Config _instance;
    public static Config Instance { get { return _instance; } }

    private StarConfig starConfig;
    private ElementConfig elementConfig;
    private UniverseConfig universeConfig;
    
    private void Awake()
    {
        if( _instance != null && _instance != this )
        {
            Destroy( this.gameObject );
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad( this.gameObject );
        _instance.Init();
        Debug.Log( "Config Awake" );
    }

    private void Init()
    {
        //has to be loaded first!
        universeConfig = new UniverseConfig();
        universeConfig.Load();

        starConfig = new StarConfig();
        starConfig.Load();

        elementConfig = new ElementConfig();
        elementConfig.Load();
    }

    public static UniverseConfig Universe
    {
        get { return _instance.universeConfig; }
    }

    public static ElementConfig Elements
    {
        get { return _instance.elementConfig; }
    }

    public static StarConfig Stars
    {
        get { return _instance.starConfig; }
    }
}