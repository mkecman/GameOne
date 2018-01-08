using LitJson;
using System;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Clock clock;
    public Player Player;
    
    [SerializeField]
    private IntReactiveProperty _Steps = new IntReactiveProperty( 100 );

    private AI ai = new AI();

    void Start()
    {
        CSV.Add( "Population,Science,Words" );

        /*
        Player.NewPlayer();
        Player.NewGalaxy();
        ai.SetPlayer( Player.PlayerModel );
        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep(1) ).AddTo( clock );
        
        _Steps.Subscribe( x => UpdateStep( x ) );

        /*
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        Debug.Log( "GameController Started" );
    }

    public void UpdateStep( int steps )
    {
        DateTime start = DateTime.Now;
        //Debug.Log( "start at: " + start.ToString() );

        for( int i = 0; i < steps; i++ )
        {
            Player.UpdateStep( 1 );
            ai.MakeMove();
        }
        DateTime end = DateTime.Now;
        //Debug.Log( "end in: " + ( end - start ).ToString() );
    }

    public void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( Player.GetPlayer(), jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Player.json", sb.ToString() );

        CSV.Save( "test.csv" );
        CSV.Add( "Done" );
        CSV.Out();
    }
}
