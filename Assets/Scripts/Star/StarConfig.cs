using UnityEngine;
using System.Collections.Generic;
using LitJson;

public class StarConfig
{
    public List<StarModel> _stars { get; set; }

    internal void Load()
    {
        Debug.Log("Load Stars");

        TextAsset targetFile = Resources.Load<TextAsset>("Configs/Stars");
        _stars = JsonMapper.ToObject<List<StarModel>>(targetFile.text);
    }
}
