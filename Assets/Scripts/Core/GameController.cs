using LitJson;
using System;
using System.Data;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private PlayerController _player;
    private GalaxyController _galaxy;
    private StarController _star;
    private PlanetController _planet;
    private LifeController _life;
    private UnitController _unit;


    public Clock clock;
//  public Player Player;
    
    [SerializeField]
    private IntReactiveProperty _Steps = new IntReactiveProperty( 100 );

    private AI ai = new AI();

    private ClockTickMessage clockTickMessage = new ClockTickMessage();

    private void Awake()
    {
        _player = new PlayerController();
        _galaxy = new GalaxyController();
        _star = new StarController();
        _planet = new PlanetController();
        _life = new LifeController();
        _unit = new UnitController();
    }

    void Start()
    {
        CSV.Add( "Population,Science,Words" );
        /*
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        StartNewGame();

        GameMessage.Listen<PlanetGenerateMessage>( OnPlanetGenerate );

        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep( 1 ) ).AddTo( clock );
        Debug.Log( "GameController Started" );
    }
    
    private void StartNewGame()
    {
        _player.New();

        _galaxy.Load( _player.Model._Galaxies );
        _galaxy.New();
        _player.Model.CreatedGalaxies++;

        _star.Load( _galaxy.SelectedGalaxy._Stars );
        _star.New( 7, _galaxy.SelectedGalaxy.CreatedStars++ );

        _planet.New( _star.SelectedStar, 0 );
        _life.New( _planet.SelectedPlanet );
        _unit.Load( _planet.SelectedPlanet );

        GameModel.Register( _planet.SelectedPlanet );
    }

    private void OnPlanetGenerate( PlanetGenerateMessage value )
    {
        LifeModel life = _planet.SelectedPlanet.Life;
        _planet.Generate( _planet.SelectedPlanet.Index );
        _planet.SelectedPlanet.Life = life;
        _life.Load( _planet.SelectedPlanet );
        _unit.Load( _planet.SelectedPlanet );
        GameModel.Register( _planet.SelectedPlanet );
    }

    public void UpdateStep( int steps )
    {
        DateTime start = DateTime.Now;
        //Debug.Log( "start at: " + start.ToString() );

        //Player.UpdateStep( steps );

        for( int i = 0; i < steps; i++ )
        {
            //ai.MakeMove();
            GameMessage.Send( clockTickMessage );
            clockTickMessage.elapsedTicksSinceStart++;
        }
        
        DateTime end = DateTime.Now;
        //Debug.Log( "end in: " + ( end - start ).ToString() );
    }

    public void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        //JsonMapper.ToJson( Player.PlayerModel, jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Player.json", sb.ToString() );

        CSV.Save( "test.csv" );
        CSV.Add( "Done" );
        CSV.Out();
    }
}
