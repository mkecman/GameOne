using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Planet
{
    private StarModel _star;
    private PlanetModel _model;
    private Life _life;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private ElementConfig _elementsConfig;

    private HexMapGenerator _hexMapGenerator;

    public Planet()
    {
        _starsConfig = Config.Get<StarsConfig>();
        _universeConfig = Config.Get<UniverseConfig>();
        _elementsConfig = Config.Get<ElementConfig>();
        _hexMapGenerator = new HexMapGenerator();
    }
    
    public PlanetModel New( StarModel star, int index, int planetCount )
    {
        _star = star;
        _model = new PlanetModel();
        _model.Name = "Planet" + star.PlanetsCount++;

        _model.Distance = GetDistance( index, planetCount );
        _model.Radius = GetRadius();
        _model.Volume = 1.333 * Math.PI * Math.Pow( _model.Radius, 3 );

        //_model.Elements = GeneratePlanetElements( index + 1, planetCount );
        //_model.Density = CalculateDensity( _model.Elements );  //( tempPlanet.Mass / tempPlanet.Volume ) / 1000; // value is in g/m3
        _model.Mass = _model.Density * _model.Volume * 1000;
        _model.Gravity = ( _universeConfig.G * _model.Mass ) / Math.Pow( _model.Radius, 2 );
        _model.OrbitalPeriod = 2 * Math.PI * Math.Sqrt( Math.Pow( _model.Distance, 3 ) / ( star.Mass * _universeConfig.G ) );
        _model.EscapeVelocity = Math.Sqrt( ( 2 * _universeConfig.G * _model.Mass ) / _model.Radius );

        _model.Pressure = 1;
        _model.MagneticField = 1;

        _model.AlbedoSurface = 0.2;
        _model.AlbedoClouds = 0.7;
        double albedo = 1 - ( ( 1 - _model.AlbedoSurface ) * ( 1 - _model.AlbedoClouds ) );
        double greenhouse = 0;

        //temperature in Kelvin
        double TemperatureFromStar = ( star.Luminosity * albedo ) / ( 16 * Math.PI * Math.Pow( _model.Distance, 2 ) * _universeConfig.Boltzmann );
        _model.Temperature = Math.Pow( ( TemperatureFromStar * ( 1 + ( ( 3 * greenhouse * 0.5841 ) / 4 ) ) / .9 ), 0.25 );
        _model.Temperature -= 273; //convert to Celsius

        _model.Map = _hexMapGenerator.Generate();

        //TODO: Remove this!!! or maybe not?
        GameModel.Set( _model );

        return _model;
    }

    public void Load( PlanetModel planetModel )
    {
        _model = planetModel;
        if( _model.Life != null )
        {
            _life = new Life();
            _life.Load( _model );
        }
        //TODO: Remove this!!! or maybe not?
        GameModel.Set( _model );
    }

    public void ActivateLife()
    {
        _life = new Life();
        _model.Life = _life.New( _model );
    }

    internal void UpdateStep( int steps )
    {
        _life.UpdateStep( steps );
    }

    private double CalculateDensity( List<PlanetElementModel> elements )
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

    private List<PlanetElementModel> GeneratePlanetElements( double index, double planetCount )
    {
        int ElementCount = 0;//_star.AvailableElements.Count;

        double curve = index * ( ElementCount * _starsConfig.MaxElementsBellCurveMagnifier / planetCount );
        double ofset = index * ( ElementCount / planetCount );

        List<PlanetElementModel> output = new List<PlanetElementModel>();
        double probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Math.Sqrt( 2 * Math.PI * curve ) ) * Math.Exp( -Math.Pow( ofset - i, 2 ) / ( 2 * curve ) ) * 1000;
            if( probability >= 1 )
            {
                PlanetElementModel planetElementModel = new PlanetElementModel
                {
                    //Index = (int)_star.AvailableElements[ i ].Value,
                    Index = 0,
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
