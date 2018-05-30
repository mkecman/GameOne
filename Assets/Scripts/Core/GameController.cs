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

        _gameControllers = new List<IGameInit>();

        _player                         = AddController<PlayerController>();
        _galaxy                         = AddController<GalaxyController>();
        _star                           = AddController<StarController>();
        _planet                         = AddController<PlanetController>();
        _life                           = AddController<LifeController>();

        AddController<UnitPaymentService>();
        AddController<UnitFactory>();
        AddController<UnitController>();
        AddController<SkillController>();
        AddController<SkillPaymentService>();
        AddController<CompoundController>();
        AddController<CompoundPaymentService>();
        //AddController<BuildingController>();
        //AddController<BuildingPaymentService>();

        AddController<HexUpdateCommand>();
        AddController<HexScoreUpdateCommand>();
        AddController<PlanetGenerateCommand>();
        AddController<PlanetPropsUpdateCommand>();
        AddController<UnitDefenseUpdateCommand>();
        AddController<SkillCommand>();
        AddController<UnitEquipCommand>();
        AddController<UnitUseCompoundCommand>();

        AddController<LiveSkill>();
        AddController<MoveSkill>();
        AddController<CloneSkill>();
        AddController<MineSkill>();

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

        if( File.Exists( Application.persistentDataPath + "-Player.json" ) )
            Observable.TimerFrame( 30 ).Subscribe( _ => Load() );
        else
            Observable.TimerFrame( 30 ).Subscribe( _ => StartNewGame() ); 

        //Load();
        //Debug.Log( "GameController Started" );
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

        clock.StartTimer();
    }

    public void Load()
    {
        _player.Model = JsonConvert.DeserializeObject<PlayerModel>( File.ReadAllText( Application.persistentDataPath + "-Player.json" ) );

        _galaxy.Load( 0 );
        _star.Load( 0 );
        _planet.Load( 0 );

        clock.StartTimer();
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

    private T AddController<T>() where T : new()
    {
        T controller = new T();
        _gameControllers.Add( controller as IGameInit );
        GameModel.Set( controller );
        return controller;
    }
}
