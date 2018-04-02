using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
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
    private UnitController _unit;
    private UnitPaymentService _unitPayment;
    private BuildingController _abilityController;
    private BuildingPaymentService _abilityPayment;

    private void Awake()
    {
        GameModel.Set( new GameDebug() );
        IsDebug.Subscribe( _ => GameModel.Get<GameDebug>().isActive = _ );

        GameModel.Set( new HexUpdateCommand() );

        _player = new PlayerController();
        _galaxy = new GalaxyController();
        _star = new StarController();
        _planet = new PlanetController();
        _life = new LifeController();
        _unit = new UnitController();
        _unitPayment = new UnitPaymentService();

        _abilityPayment = new BuildingPaymentService();
        GameModel.Set( _abilityPayment );

        _abilityController = new BuildingController();

        GameModel.Set( _player );
        GameModel.Set( _galaxy );
        GameModel.Set( _star );
        GameModel.Set( _planet );
        GameModel.Set( _life );
        GameModel.Set( _unit );
        GameModel.Set( _unitPayment );
        GameModel.Set( _abilityController );
        
        GameModel.Set( new PlanetGenerateCommand() );
    }

    void Start()
    {
        /*
        CSV.Add( "Population,Science,Words" );
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        //StartNewGame();
        Debug.Log( "GameController Started" );
    }
    
    public void StartNewGame()
    {
        _player.New();

        _galaxy.SetModel( _player.Model._Galaxies );
        _galaxy.New();

        _player.Model.CreatedGalaxies++;

        _star.SetModel( _galaxy.SelectedGalaxy._Stars );
        _star.New( 7, _galaxy.SelectedGalaxy.CreatedStars++ );

        _planet.New( _star.SelectedStar, 0 );
        _life.New( _planet.SelectedPlanet );

        GameObject go = GameObject.Find( "Map" );
        HexMap hexMap = go.GetComponent<HexMap>();
        GameModel.Get<PlanetGenerateCommand>().Execute( hexMap );

        clock.ElapsedUpdates.Subscribe<long>( x => UpdateStep( 1 ) ).AddTo( clock ); //start the game ticking
    }
    
    public void Load()
    {
        _player.Model = JsonConvert.DeserializeObject<PlayerModel>( File.ReadAllText( Application.persistentDataPath + "-Player.json" ) );

        _galaxy.SetModel( _player.Model._Galaxies );
        _galaxy.Load( 0 );

        _star.SetModel( _galaxy.SelectedGalaxy._Stars );
        _star.Load( 0 );

        _planet.Load( _star.SelectedStar, 0 );
        _life.Load( _planet.SelectedPlanet );

        GameModel.Set<PlanetModel>( _planet.SelectedPlanet );

        _unit.Load( _planet.SelectedPlanet );

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

    public void UpdateStep( int steps )
    {
        DateTime start = DateTime.Now;
        //Debug.Log( "start at: " + start.ToString() );

        for( int i = 0; i < steps; i++ )
        {
            GameMessage.Send( clock.message );
            clock.message.elapsedTicksSinceStart++;
        }

        DateTime end = DateTime.Now;
        //Debug.Log( "end in: " + ( end - start ).ToString() );
    }
}
