using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class CompoundEditor : MonoBehaviour
{
    public List<CompoundJSON> List;
    
    public void Load()
    {
        List = GameConfig.Get<CompoundConfig>();
    }

    public void Save()
    {
        File.WriteAllText(
            "Assets/Resources/Configs/CompoundConfig.json",
            JsonConvert.SerializeObject( List )
            );
        Debug.Log( "CompoundConfig Saved" );

        //UnityEditor.AssetDatabase.Refresh(); //this is not to forget how to refresh assets after change.. you still need to reload the game to take new configs!
    }
}
