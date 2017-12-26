using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameController : MonoBehaviour
{
    public Clock clock;
    private PlayerManager _player;

    
    void Start()
    {

        _player = new PlayerManager();

        _player.NewPlayer();
        _player.NewGalaxy();
        
        clock._ElapsedUpdates.Subscribe<long>( x => UpdateStep() ).AddTo(clock);
        /*
        //ElementConfigGenerator generator = new ElementConfigGenerator();
        //generator.Load();
        */
            
        CSV.Add( "Population,Science" );
        Debug.Log( "GameController Started" );
    }
    
    public void UpdateStep()
    {
        _player.UpdateStep(1);
    }
    
    public void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( _player.GetPlayer(), jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Player.json", sb.ToString() );

        CSV.Save( "test.csv" );
        CSV.Add( "Done" );
        CSV.Out();
    }
}
