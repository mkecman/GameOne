using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    PlayerManager _player;
    
    void Start()
    {
        _player = new PlayerManager();
        _player.NewPlayer();

        //ElementConfigGenerator generator = new ElementConfigGenerator();
        //generator.Load();

        Debug.Log( "GameController Start" );
    }

    public void UpdateStep()
    {
        _player.UpdateStep( 100 );
    }

    public void Save()
    {
        Log.Save( "test.csv" );
        Log.Add( "Done" );
        Log.Out();
    }

}
