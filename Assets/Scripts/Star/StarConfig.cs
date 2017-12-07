﻿using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System;

public class StarConfig
{
    private List<StarModel> _stars { get; set; }
    private GeneralStarConfig _general { get; set; }

    public void Load()
    {
        Debug.Log("Load Stars");

        TextAsset targetFile = Resources.Load<TextAsset>("Configs/Stars");
        _stars = JsonMapper.ToObject<List<StarModel>>(targetFile.text);

        TextAsset targetFile2 = Resources.Load<TextAsset>("Configs/GeneralStarConfig");
        _general = JsonMapper.ToObject<GeneralStarConfig>(targetFile2.text);
    }

    internal StarModel Get( double Words )
    {
        return _stars[ (int)Words ];
    }

    internal GeneralStarConfig Settings
    {
        get { return _general; }
    }
}
