using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance { get { return _instance; } }
    private static Config _instance;
    private Dictionary<string, object> _configs;
    
    private void Init()
    {
        _instance._configs = new Dictionary<string, object>();

        _instance.Load<UniverseConfig>();
        _instance.Load<StarsConfig>();
        _instance.Load<ElementConfig>();
        _instance.Load<HexConfig>();
        _instance.Load<AbilityConfig>();
        Get<AbilityConfig>().Setup();
    }

    public static T Get<T>()
    {
        string className = typeof( T ).Name;
        if( _instance._configs.ContainsKey( className ) )
            return (T)_instance._configs[ className ];
        else
            return default( T );
    }

    private void Load<T>()
    {
        string className = typeof( T ).Name;
        TextAsset configFile = Resources.Load<TextAsset>( "Configs/" + className );
        if( configFile != null )
        {
            JsonMapper.RegisterExporter<float>( ( obj, writer ) => writer.Write( Convert.ToDouble( obj ) ) );
            JsonMapper.RegisterImporter<double, float>( input => Convert.ToSingle( input ) );
            _instance._configs.Add( className, JsonMapper.ToObject<T>( configFile.text ) );
            Debug.Log( "Loaded config: " + className );
        }
    }

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
        Debug.Log( "Config Awaken" );
    }
    
}