using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class UniverseConfig
{
    public UniverseModel Data { get; set; }

    internal void Load()
    {
        Debug.Log( "Load Universe" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Universe" );
        Data = JsonMapper.ToObject<UniverseModel>( targetFile.text );
    }
}
