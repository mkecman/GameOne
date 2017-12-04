﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class ElementConfig
{
    public List<ElementModel> elements { get; set; }

    internal void Load()
    {
        Debug.Log( "Load Elements" );

        TextAsset targetFile = Resources.Load<TextAsset>( "Configs/Elements" );
        elements = JsonMapper.ToObject<List<ElementModel>>( targetFile.text );
    }
}
