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
    private UnitPaymentService _unitPayment;
    private AbilityController _abilityController;
    private AbilityPaymentService _abilityPayment;
    public Clock clock;

    private void Awake()
    {
        GameModel.Set( new HexUpdateCommand() );

        _player = new PlayerController();
        _galaxy = new GalaxyController();
        _star = new StarController();
        _planet = new PlanetController();
        _life = new LifeController();
        _unit = new UnitController();
        _unitPayment = new UnitPaymentService();

        _abilityPayment = new AbilityPaymentService();
        GameModel.Set( _abilityPayment );

        _abilityController = new AbilityController();

        GameModel.Set( _player );
        GameModel.Set( _galaxy );
        GameModel.Set( _star );
        GameModel.Set( _planet );
        GameModel.Set( _life );
        GameModel.Set( _unit );
        GameModel.Set( _unitPayment );
        GameModel.Set( _abilityController );
        
        GameCommand.Register( new PlanetGenerateCommand() );
    }

    void Start()
    {
        /*
        CSV.Add( "Population,Science,Words" );
        ElementConfigGenerator generator = new ElementConfigGenerator();
        generator.Load();
        /**/

        StartNewGame();

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

        GameModel.Set( _planet.SelectedPlanet );
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

    public void Save()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter( sb );
        jsonWriter.PrettyPrint = true;
        JsonMapper.ToJson( _player.Model, jsonWriter );
        File.WriteAllText( Application.persistentDataPath + "-Player.json", sb.ToString() );

        CSV.Save( "test.csv" );
        CSV.Add( "Done" );
        CSV.Out();
    }
}
