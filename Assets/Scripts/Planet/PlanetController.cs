using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;

public class PlanetController : AbstractController, IGameInit
{
    public PlanetModel SelectedPlanet { get { return _selectedPlanet; } }

    private StarModel _star;
    private PlanetModel _selectedPlanet;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private HexMapGenerator _hexMapGenerator;
    private PlanetPropsUpdateCommand _planetUpdateCommand;
    private int _counter;

    public void Init()
    {
        _starsConfig = GameConfig.Get<StarsConfig>();
        _universeConfig = GameConfig.Get<UniverseConfig>();
        _hexMapGenerator = new HexMapGenerator();
        _planetUpdateCommand = GameModel.Get<PlanetPropsUpdateCommand>();

        GameModel.HandleGet<StarModel>( OnStarChange );
    }

    private void OnStarChange( StarModel value )
    {
        _star = value;
    }

    public void Load( int index )
    {
        _selectedPlanet = _star._Planets[ index ];
        PlanetLoaded();
    }
    
    public void New( int index )
    {
        Generate( index );
        _star._Planets.Add( _selectedPlanet );
        _star.PlanetsCount++;
        PlanetLoaded();
    }

    public void GenerateFromModel( PlanetModel planetModel )
    {
        planetModel.Map = _hexMapGenerator.Generate( planetModel );
        _selectedPlanet = planetModel;
    }

    public void PlanetLoaded()
    {
        _planetUpdateCommand.Execute();
        GameModel.Set<PlanetModel>( _selectedPlanet );
        GameMessage.Listen<ClockTickMessage>( OnClockTick );
    }

    private void OnClockTick( ClockTickMessage value )
    {
        _planetUpdateCommand.Execute();

        if( _counter >= 30 ) //update every 30th tick (seconds)
        {
            _counter = 0;
        }
        _counter++;
    }

    private void Generate( int index )
    {
        _selectedPlanet = new PlanetModel();
        _selectedPlanet.Name = "Planet " + index;
        _selectedPlanet.Index = index;

        _selectedPlanet.Distance = GetDistance( index, _star.PlanetsCount );
        _selectedPlanet.Radius = GetRadius();
        _selectedPlanet.Volume = 1.333f * Mathf.PI * Mathf.Pow( _selectedPlanet.Radius, 3 );
        _selectedPlanet.Mass = _selectedPlanet.Density * _selectedPlanet.Volume * 1000;

        //_selectedPlanet._Elements = GeneratePlanetElements( index + 1, _star.PlanetsCount );
        //_selectedPlanet.Density = CalculateDensity( _selectedPlanet._Elements );  //( tempPlanet.Mass / tempPlanet.Volume ) / 1000; // value is in g/m3
        _selectedPlanet.Density = ( _selectedPlanet.Mass / _selectedPlanet.Volume ) / 1000;
        _selectedPlanet.Gravity = ( _universeConfig.G * _selectedPlanet.Mass ) / Mathf.Pow( _selectedPlanet.Radius, 2 );
        _selectedPlanet.OrbitalPeriod = 2 * Mathf.PI * Mathf.Sqrt( Mathf.Pow( _selectedPlanet.Distance, 3 ) / ( _star.Mass * _universeConfig.G ) );
        _selectedPlanet.EscapeVelocity = Mathf.Sqrt( ( 2 * _universeConfig.G * _selectedPlanet.Mass ) / _selectedPlanet.Radius );

        _selectedPlanet.Pressure = 1;
        _selectedPlanet.MagneticField = 1;

        _selectedPlanet.AlbedoSurface = 0.2f;
        _selectedPlanet.AlbedoClouds = 0.7f;
        float albedo = 1 - ( ( 1 - _selectedPlanet.AlbedoSurface ) * ( 1 - _selectedPlanet.AlbedoClouds ) );
        float greenhouse = 0;

        //temperature in Kelvin
        float TemperatureFromStar = ( _star.Luminosity * albedo ) / ( 16 * Mathf.PI * Mathf.Pow( _selectedPlanet.Distance, 2 ) * _universeConfig.Boltzmann );
        _selectedPlanet.Props[ R.Temperature ].Value = Mathf.Pow( ( TemperatureFromStar * ( 1 + ( ( 3 * greenhouse * 0.5841f ) / 4 ) ) / .9f ), 0.25f );
        _selectedPlanet.Props[ R.Temperature ].Value -= 273; //convert to Celsius

        _selectedPlanet.Map = _hexMapGenerator.Generate( _selectedPlanet );
    }
    
    private float GetRadius()
    {
        return RandomUtil.GetRandomWeightedValue( _starsConfig.MinPlanetaryRadiusInMeters, _starsConfig.PlanetaryRadiusInMeters );
    }

    private float GetDistance( int index, int planetCount )
    {
        if( planetCount > 5 )
            return _starsConfig.TenPlanetDistancesInAU[ index ] * _star.HabitableZone;
        else
            return _starsConfig.FivePlanetDistancesInAU[ index ] * _star.HabitableZone;
    }

}
