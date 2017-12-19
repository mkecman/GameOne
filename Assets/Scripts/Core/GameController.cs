using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        _player.NewGalaxy();

        //ElementConfigGenerator generator = new ElementConfigGenerator();
        //generator.Load();

        Log.Add( "Population,Science", true );
        Debug.Log( "GameController Start" );
    }

    public void UpdateStep()
    {
        _player.UpdateStep( 10 );
    }
    
    public void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( _player.GetPlayer(), jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Player.json", sb.ToString() );

        Log.Save( "test.csv" );
        Log.Add( "Done" );
        Log.Out();
    }
}
