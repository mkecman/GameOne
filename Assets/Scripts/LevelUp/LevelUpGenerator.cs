using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LevelUpGenerator : MonoBehaviour
{
    private List<LevelUpModel> _levels = new List<LevelUpModel>();
    private float BaseXP = 10f;
    private float Exponent = 1.5f;
    private float UpgradeDivisor = 34f;

    // Use this for initialization
    void Start()
    {
        for( int i = 0; i < 100; i++ )
        {
            LevelUpModel level = new LevelUpModel();
            level.Level = i;
            level.Experience = Mathf.RoundToInt( Mathf.Pow( i * BaseXP, Exponent ) );
            level.UpgradePoints = Mathf.CeilToInt( i / UpgradeDivisor );

            _levels.Add( level );
        }

        //SAVE TO FILE
        File.WriteAllText(
            Application.persistentDataPath + "-LevelUpConfig.json",
            JsonConvert.SerializeObject( _levels )
            );

        Debug.Log( "DONE Generating LevelUpConfig" );
    }

}
