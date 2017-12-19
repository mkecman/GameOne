using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text from;
    public Text to;
    private PlayerManager _player;

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
        _player.UpdateStep( 10 );
    }

    public void Move()
    {
        MoveWorker( int.Parse(from.text), int.Parse( to.text ) );
    }

    private void MoveWorker( int from, int to )
    {
        _player.MoveWorker( from, to );
    }

    public void Save()
    {
        Log.Save( "test.csv" );
        Log.Add( "Done" );
        Log.Out();
    }

}
