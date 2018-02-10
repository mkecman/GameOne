using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;

public class PlanetController : AbstractController
{
    public PlanetModel SelectedPlanet { get { return _selectedPlanet; } }

    private StarModel _star;
    private PlanetModel _selectedPlanet;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private ElementConfig _elementsConfig;
    private HexMapGenerator _hexMapGenerator;

    public PlanetController()
    {
        _starsConfig = Config.Get<StarsConfig>();
        _universeConfig = Config.Get<UniverseConfig>();
        _elementsConfig = Config.Get<ElementConfig>();
        _hexMapGenerator = new HexMapGenerator();
    }
    
    public void Load( StarModel star, int index )
    {
        _star = star;
        _selectedPlanet = _star._Planets[ index ];
    }

    public void New( StarModel star, int index )
    {
        _star = star;
        Generate( index );
        _star._Planets.Add( _selectedPlanet );
    }

    public void Generate( int index )
    {
        _selectedPlanet = new PlanetModel();
        _selectedPlanet.Name = "Planet " + index;
        _selectedPlanet.Index = index;

        _selectedPlanet.Distance = GetDistance( index, _star.PlanetsCount );
        _selectedPlanet.Radius = GetRadius();
        _selectedPlanet.Volume = 1.333 * Math.PI * Math.Pow( _selectedPlanet.Radius, 3 );

        _selectedPlanet._Elements = GeneratePlanetElements( index + 1, _star.PlanetsCount );
        _selectedPlanet.Density = CalculateDensity( _selectedPlanet._Elements );  //( tempPlanet.Mass / tempPlanet.Volume ) / 1000; // value is in g/m3
        _selectedPlanet.Mass = _selectedPlanet.Density * _selectedPlanet.Volume * 1000;
        _selectedPlanet.Gravity = ( _universeConfig.G * _selectedPlanet.Mass ) / Math.Pow( _selectedPlanet.Radius, 2 );
        _selectedPlanet.OrbitalPeriod = 2 * Math.PI * Math.Sqrt( Math.Pow( _selectedPlanet.Distance, 3 ) / ( _star.Mass * _universeConfig.G ) );
        _selectedPlanet.EscapeVelocity = Math.Sqrt( ( 2 * _universeConfig.G * _selectedPlanet.Mass ) / _selectedPlanet.Radius );

        _selectedPlanet.Pressure = 1;
        _selectedPlanet.MagneticField = 1;

        _selectedPlanet.AlbedoSurface = 0.2;
        _selectedPlanet.AlbedoClouds = 0.7;
        double albedo = 1 - ( ( 1 - _selectedPlanet.AlbedoSurface ) * ( 1 - _selectedPlanet.AlbedoClouds ) );
        double greenhouse = 0;

        //temperature in Kelvin
        double TemperatureFromStar = ( _star.Luminosity * albedo ) / ( 16 * Math.PI * Math.Pow( _selectedPlanet.Distance, 2 ) * _universeConfig.Boltzmann );
        _selectedPlanet.Temperature = Math.Pow( ( TemperatureFromStar * ( 1 + ( ( 3 * greenhouse * 0.5841 ) / 4 ) ) / .9 ), 0.25 );
        _selectedPlanet.Temperature -= 273; //convert to Celsius

        _selectedPlanet.Map = _hexMapGenerator.Generate();
    }

    private double CalculateDensity( ReactiveCollection<PlanetElementModel> elements )
    {
        double totalDensity = 0;
        double totalAmount = 0;
        int elementsCount = elements.Count;
        for( int i = 0; i < elementsCount; i++ )
        {
            totalDensity += _elementsConfig.Elements[ elements[ i ].Index ].Density * elements[ i ].Amount;
            totalAmount += elements[ i ].Amount;
        }

        return totalDensity / totalAmount;
    }

    private ReactiveCollection<PlanetElementModel> GeneratePlanetElements( double index, double planetCount )
    {
        int ElementCount = _star._AvailableElements.Count;

        double curve = index * ( ElementCount * _starsConfig.MaxElementsBellCurveMagnifier / planetCount );
        double ofset = index * ( ElementCount / planetCount );

        ReactiveCollection<PlanetElementModel> output = new ReactiveCollection<PlanetElementModel>();
        double probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Math.Sqrt( 2 * Math.PI * curve ) ) * Math.Exp( -Math.Pow( ofset - i, 2 ) / ( 2 * curve ) ) * 1000;
            if( probability >= 1 )
            {
                PlanetElementModel planetElementModel = new PlanetElementModel
                {
                    Index = (int)_star._AvailableElements[ i ].Value,
                    Amount = probability
                };

                output.Add( planetElementModel );
            }
        }

        return output;
    }

    private double GetRadius()
    {
        return GetRandomWeightedValue( _starsConfig.MinPlanetaryRadiusInMeters, _starsConfig.PlanetaryRadiusInMeters );
    }

    private double GetDistance( int index, int planetCount )
    {
        if( planetCount > 5 )
            return _starsConfig.TenPlanetDistancesInAU[ index ] * _star.HabitableZone;
        else
            return _starsConfig.FivePlanetDistancesInAU[ index ] * _star.HabitableZone;
    }

    private double GetRandomWeightedValue( double minValue, List<WeightedValue> values )
    {
        double min;
        int key = RandomUtil.GetWeightedKey( values );
        if( key == 0 )
            min = minValue;
        else
            min = values[ key - 1 ].Value;

        double max = values[ key ].Value;

        return RandomUtil.FromRange( min, max );
    }
}
