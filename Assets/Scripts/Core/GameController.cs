using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Clock clock;
    public BoolReactiveProperty IsDebug = new BoolReactiveProperty();

    public int UpdateSteps = 360;
    public BoolReactiveProperty RunSteps = new BoolReactiveProperty();

    private PlayerController _player;
    private GalaxyController _galaxy;
    private StarController _star;
    private PlanetController _planet;
    private LifeController _life;
    
    private List<IGameInit> _gameControllers;

    private void Awake()
    {
        GameModel.Set( new GameDebug() );
        IsDebug.Subscribe( _ => GameModel.Get<GameDebug>().isActive = _ );
        RunSteps.Where( _ => _ == true ).Subscribe( _ => StopwatchSteps( UpdateSteps ) );

        _gameControllers = new List<IGameInit>();

        _player                         = AddController<PlayerController>();
        _galaxy                         = AddController<GalaxyController>();
        _star                           = AddController<StarController>();
        _planet                         = AddController<PlanetController>();
        _life                           = AddController<LifeController>();

        AddController<UnitController>();
        AddController<UnitPaymentService>();
        AddController<BuildingController>();
        AddController<BuildingPaymentService>();
        AddController<HexUpdateCommand>();
        AddController<PlanetGenerateCommand>();
        AddController<PlanetPropsUpdateCommand>();

        for( int i = 0; i < _gameControllers.Count; i++ )
        {
            _gameControllers[ i ].Init();
        }
    }
    
    void Start()
    {
        /*
        CSV.Add( "Population,Science,Words" );
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        //StartNewGame();
        //Load();
        Debug.Log( "GameController Started" );
    }

    public void StartNewGame()
    {
        _player.New();
        _galaxy.New();
        _star.New( 7 );
        _planet.New( 0 );
        _life.New();

        GameObject go = GameObject.Find( "Map" );
        HexMap hexMap = go.GetComponent<HexMap>();
        GameModel.Get<PlanetGenerateCommand>().Execute( hexMap );

        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep( 1 ) ).AddTo( clock ); //start the game ticking
    }

    public void Load()
    {
        _player.Model = JsonConvert.DeserializeObject<PlayerModel>( File.ReadAllText( Application.persistentDataPath + "-Player.json" ) );

        _galaxy.Load( 0 );
        _star.Load( 0 );
        _planet.Load( 0 );

        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep( 1 ) ).AddTo( clock );
    }

    public void Save()
    {
        File.WriteAllText(
            Application.persistentDataPath + "-Player.json",
            JsonConvert.SerializeObject( _player.Model )
            );
        Debug.Log( "Saved game" );

        /*
        CSV.Save( "test.csv" );
        CSV.Add( "Done" );
        CSV.Out();
        */
    }

    private void StopwatchSteps( int steps )
    {
        DateTime start = DateTime.Now;
        //Debug.Log( "start at: " + start.ToString() );

        UpdateStep( steps );

        DateTime end = DateTime.Now;
        Debug.Log( "end in: " + ( end - start ).ToString() );
    }

    public void UpdateStep( int steps )
    {
        for( int i = 0; i < steps; i++ )
        {
            GameMessage.Send( clock.message );
            clock.message.elapsedTicksSinceStart++;
        }
    }

    private T AddController<T>() where T : new()
    {
        T controller = new T();
        _gameControllers.Add( controller as IGameInit );
        GameModel.Set( controller );
        return controller;
    }
}
