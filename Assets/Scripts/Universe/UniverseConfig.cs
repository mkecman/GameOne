using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class UniverseConfig
{
    public UniverseModel universe { get; set; }

    internal void Load()
    {
        Debug.Log( "Load Universe" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Universe" );
        universe = JsonMapper.ToObject<UniverseModel>( targetFile.text );
    }
}
