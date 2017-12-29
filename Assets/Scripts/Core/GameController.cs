using LitJson;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Clock clock;
    public Player Player;


    void Start()
    {
        Player.NewPlayer();
        Player.NewGalaxy();
        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep() ).AddTo( clock );

        /*
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        CSV.Add( "Population,Science" );
        Debug.Log( "GameController Started" );
    }

    public void UpdateStep()
    {
        Player.UpdateStep( 1 );
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
