using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LevelUpGenerator : MonoBehaviour
{
    private List<LevelUpModel> _levels = new List<LevelUpModel>();
    private float BaseXPMultiplier = 2f;
    private int BaseXP = 10;
    private float Exponent = 1.5f;

    private float BaseHealth = 100f;
    private float ExponentHealth = 0.7f;

    private float UpgradeDivisor = 34f;

    // Use this for initialization
    void Start()
    {
        for( int i = 0; i < 100; i++ )
        {
            LevelUpModel level = new LevelUpModel();
            level.Level = i;
            level.Experience = Mathf.RoundToInt( Mathf.Pow( i * BaseXPMultiplier, Exponent ) ) + BaseXP;
            level.UpgradePoints = Mathf.CeilToInt( i / UpgradeDivisor );
            level.Effects.Add( R.Health, Mathf.RoundToInt( BaseHealth + Mathf.Pow( i * BaseHealth, ExponentHealth ) ) );

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
