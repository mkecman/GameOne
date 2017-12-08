using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class UniverseConfig
{
    public UniverseModel Constants { get; set; }

    internal void Load()
    {
        Debug.Log( "Load Universe" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Universe" );
        Constants = JsonMapper.ToObject<UniverseModel>( targetFile.text );
    }
}
