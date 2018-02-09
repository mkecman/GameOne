using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class Star
{
    private StarModel _model;
    private List<Planet> _planets;
    private List<Planet> _colonizedPlanets;

    private StarsConfig _starsConfig;
    private UniverseConfig _universeConfig;
    private ElementConfig _elementsConfig;

    public Star()
    {
        _starsConfig = Config.Get<StarsConfig>();
        _universeConfig = Config.Get<UniverseConfig>();
        _elementsConfig = Config.Get<ElementConfig>();
    }
    
    public StarModel New( int Type, int Index )
    {
        _model = GameModel.Copy<StarModel>( _starsConfig.Stars[ Type ] );
        ConvertUnitsToSI();
        _model.Name = "Star " + Index;
        _model.AvailableElements = GenerateStarElements( _model.Index );
        GeneratePlanets( Type );
        _colonizedPlanets = new List<Planet>();

        //TODO: Remove this
        ActivateLifeOnPlanet( 0 );

        return _model;
    }

    public void Load( StarModel starModel )
    {
        _model = starModel;
        ConvertUnitsToSI();
        _colonizedPlanets = new List<Planet>();
        for( int i = 0; i < _model.Planets.Count; i++ )
        {
            LoadPlanet( _model.Planets[ i ] );
        }
    }

    public void ActivateLifeOnPlanet( int planetIndex )
    {
        _planets[ planetIndex ].ActivateLife();
        _colonizedPlanets.Add( _planets[ planetIndex ] );
    }

    public void UpdateStep( int steps )
    {
        _model.Lifetime -= steps;

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
        _model.Planets = new List<PlanetModel>();
        _planets = new List<Planet>();

        int planetCount = RandomUtil.FromRangeInt( _starsConfig.MinPlanets, _starsConfig.MaxPlanets );
        planetCount = 1;

        for ( int index = 0; index < planetCount; index++ )
        {
            Planet planet = new Planet();
            _model._Planets.Add( planet.New( _model, index, planetCount ) );
            _planets.Add( planet );
        }
    }
    
    private List<WeightedValue> GenerateStarElements( int index )
    {
        int ElementCount = _elementsConfig.Elements.Count;

        double curve = index * ( ElementCount * _starsConfig.MaxElementsBellCurveMagnifier / _starsConfig.Stars.Count );
        double ofset = index * ( ElementCount / _starsConfig.Stars.Count );

        List<WeightedValue> output = new List<WeightedValue>();
        double probability;
        for( int i = 0; i < ElementCount; i++ )
        {
            probability = ( 1 / Math.Sqrt( 2 * Math.PI * curve ) ) * Math.Exp( -Math.Pow( ofset - i, 2 ) / ( 2 * curve ) );
            if( probability >= .01 )
            {
                WeightedValue element = new WeightedValue
                {
                    Value = i + 1,
                    Weight = probability
                };
                output.Add( element );
            }
        }

        return output;
    }
    
    private void ConvertUnitsToSI()
    {
        _model.Mass               *= _universeConfig.SunMassInKilograms;
        _model.Radius             *= _universeConfig.SunRadiusInMeters;
        _model.HabitableZone      *= _universeConfig.AUInMeters;
        _model.InnerHabitableZone *= _universeConfig.AUInMeters;
        _model.OuterHabitableZone *= _universeConfig.AUInMeters;
        _model.Luminosity         *= _universeConfig.SunLuminosityInWatts;
    }
}
