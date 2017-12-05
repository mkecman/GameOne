using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System;

public class StarConfig
{
    public List<StarModel> _stars { get; set; }

    public void Load()
    {
        Debug.Log("Load Stars");

        TextAsset targetFile = Resources.Load<TextAsset>("Configs/Stars");
        _stars = JsonMapper.ToObject<List<StarModel>>(targetFile.text);
    }

    internal StarModel Get( double Words )
    {
        return _stars[ (int)Words ];
    }
}
