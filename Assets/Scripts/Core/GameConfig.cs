using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static GameConfig Instance { get { return _instance; } }
    private static GameConfig _instance;
    private Dictionary<string, object> _configs;
    
    private void Init()
    {
        _instance._configs = new Dictionary<string, object>();

        _instance.Load<UniverseConfig>();
        _instance.Load<StarsConfig>();

        _instance.Load<ElementConfig>();
        Get<ElementConfig>().Setup();

        _instance.Load<BodySlotsConfig>();
        _instance.Load<HexConfig>();

        _instance.Load<CompoundConfig>();
        Get<CompoundConfig>().Setup();

        _instance.Load<SkillConfig>();
        _instance.Load<BellCurveConfig>();
        _instance.Load<BuildingConfig>();
        //Get<BuildingConfig>().Setup();

        _instance.Load<LevelUpConfig>();
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
            _instance._configs.Add( className, JsonConvert.DeserializeObject<T>( configFile.text ) );
            //Debug.Log( "Loaded config: " + className );
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
        //Debug.Log( "Config Awaken" );
    }
    
}