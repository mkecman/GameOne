﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class Star
{
    private StarModel _star;
    private List<Planet> _planets;
    private List<Planet> _colonizedPlanets;

    public Star()
    {
    }
    
    public StarModel New( int Type, int Index )
    {
        _star = Config.Stars.Get( Type );
        _star.Name = "Star " + Index;
        _star.AvailableElements = GenerateStarElements( _star.Index );
        GeneratePlanets( Type );
        _colonizedPlanets = new List<Planet>();

        //TODO: Remove this
        ActivateLifeOnPlanet( 0 );

        return _star;
    }

    public void Load( StarModel starModel )
    {
        _star = starModel;
        _colonizedPlanets = new List<Planet>();
        for( int i = 0; i < _star.Planets.Count; i++ )
        {
            LoadPlanet( _star.Planets[ i ] );
        }
    }

    public void ActivateLifeOnPlanet( int planetIndex )
    {
        _planets[ planetIndex ].ActivateLife();
        _colonizedPlanets.Add( _planets[ planetIndex ] );
    }

    public void UpdateStep( ulong steps )
    {
        _star.Lifetime -= steps;

        int length = _colonizedPlanets.Count;
        for( int i = 0; i < length; i++ )
        {
            _colonizedPlanets[ i ].UpdateStep( steps );
        }
    }
    
    private void LoadPlanet( PlanetModel planetModel )
    {
        Planet planet = new Planet();
        planet.Load( planetModel );
        _planets.Add( planet );

        if( planetModel.Life != null )
        {
            _colonizedPlanets.Add( planet );
        }
    }
    
    private void GeneratePlanets( double Words )
    {
        _star.Planets = new List<PlanetModel>();
        _planets = new List<Planet>();

        int planetCount = RandomUtil.FromRangeInt( Config.Stars.Settings.MinPlanets, Config.Stars.Settings.MaxPlanets );
        planetCount = Config.Stars.Settings.MaxPlanets;

        //Log.Add( "-1,-1,-1,-1,", true );

        for ( int index = 0; index < planetCount; index++ )
        {
            PlanetModel tempPlanet = new PlanetModel();
            tempPlanet.Name = "Planet" + _star.CreatedPlanets++;

            
            tempPlanet.Distance = GetDistance(index, planetCount);
            tempPlanet.Radius = GetRadius();
            tempPlanet.Volume = 1.333*Math.PI * Math.Pow( tempPlanet.Radius, 3);

            tempPlanet.Elements = GeneratePlanetElements( index+1, planetCount );
            tempPlanet.Density = CalculateDensity( tempPlanet.Elements );  //( tempPlanet.Mass / tempPlanet.Volume ) / 1000; // value is in g/m3
            tempPlanet.Mass = tempPlanet.Density * tempPlanet.Volume * 1000;
            tempPlanet.Gravity = ( Config.Universe.G * tempPlanet.Mass ) / Math.Pow( tempPlanet.Radius, 2 );
            tempPlanet.OrbitalPeriod = 2*Math.PI * Math.Sqrt( Math.Pow( tempPlanet.Distance, 3) / ( _star.Mass * Config.Universe.G ) );
            tempPlanet.EscapeVelocity = Math.Sqrt( ( 2*Config.Universe.G * tempPlanet.Mass ) / tempPlanet.Radius );
            
            tempPlanet.Pressure = 1;
            tempPlanet.MagneticField = 1;

            tempPlanet.AlbedoSurface = 0.2;
            tempPlanet.AlbedoClouds = 0.7;
            double albedo = 1 - ( ( 1 - tempPlanet.AlbedoSurface )*( 1 - tempPlanet.AlbedoClouds ) );
            double greenhouse = 0;
            
            //temperature in Kelvin
            double TemperatureFromStar = ( _star.Luminosity * albedo ) / ( 16 * Math.PI * Math.Pow( tempPlanet.Distance, 2 ) * Config.Universe.Boltzmann );
            tempPlanet.Temperature = Math.Pow( ( TemperatureFromStar * ( 1 + ( ( 3 * greenhouse * 0.5841 ) / 4 ) ) / .9 ), 0.25 );
            tempPlanet.Temperature -= 273; //convert to Celsius

            _star.Planets.Add(tempPlanet);

            Planet planet = new Planet();
            planet.Load( tempPlanet );
            _planets.Add( planet );

            //Log.Add( tempPlanet.Density + "," + tempPlanet.Mass + "," + tempPlanet.Radius + "," + tempPlanet.Gravity + "," + tempPlanet.Temperature + "," + tempPlanet.Distance + ",", true );

            //Mass of a planet based on it's satellite orbital period!
            //( 4 * Math.Pow( Math.PI, 2) * Math.Pow(tempPlanet.Radius * 100, 3)) / ( Math.Pow(SATELLITE.OrbitalPeriod, 2) * ( Config.Universe.Constants.G / 1000 ) );
        }
    }

    private double CalculateDensity( List<PlanetElementModel> elements )
    {
        double totalDensity = 0;
        double totalAmount = 0;
        int elementsCount = elements.Count;
        for( int i = 0; i < elementsCount; i++ )
        {
            totalDensity += Config.Elements[ elements[ i ].Index ].Density * elements[ i ].Amount;
            totalAmount += elements[ i ].Amount;
        }
        
        return totalDensity / totalAmount;
    }
    
    private List<WeightedValue> GenerateStarElements( int index )
    {
        int ElementCount = Config.Elements.Count;

        double curve = index * ( ElementCount * Config.Stars.Settings.MaxElementsBellCurveMagnifier / Config.Stars.StarTypesCount );
        double ofset = index * ( ElementCount / Config.Stars.StarTypesCount );

        List<WeightedValue> output = new List<WeightedValue>();
        double probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Math.Sqrt( 2 * Math.PI * curve ) ) * Math.Exp( -Math.Pow( ofset - i, 2 ) / ( 2 * curve ) );
            if( probability >= .01 )
            {
                WeightedValue element = new WeightedValue
                {
                    Value = i,
                    Weight = probability
                };
                output.Add( element );
            }
        }

        return output;
    }

    private List<PlanetElementModel> GeneratePlanetElements( double index, double planetCount )
    {
        int ElementCount = _star.AvailableElements.Count;

        double curve = index * ( ElementCount * Config.Stars.Settings.MaxElementsBellCurveMagnifier / planetCount );
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
                    Index = (int)_star.AvailableElements[ i ].Value,
                    Amount = probability
                };

                output.Add( planetElementModel );
            }
        }
        
        return output;
    }

    private double GetRadius()
    {
        return GetRandomWeightedValue( Config.Stars.Settings.MinPlanetaryRadiusInMeters, Config.Stars.Settings.PlanetaryRadiusInMeters );
    }

    private double GetMass()
    {
        return GetRandomWeightedValue( Config.Stars.Settings.MinPlanetaryMassInKilograms, Config.Stars.Settings.PlanetaryMassInKilograms );
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

    private double GetDistance(int index, int planetCount)
    {
        if (planetCount > 5)
            return Config.Stars.Settings.TenPlanetDistancesInAU[index] * _star.HabitableZone;
        else
            return Config.Stars.Settings.FivePlanetDistancesInAU[index] * _star.HabitableZone;
    }
}
