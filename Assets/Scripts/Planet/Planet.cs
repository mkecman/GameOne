using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Planet
{
    private PlanetModel _planet;
    private LifeModel _life;
    private Dictionary<string, double> _updateValues;

    public Planet( PlanetModel model )
    {
        _planet = model;
        _updateValues = new Dictionary<string, double>()
        {
            { ElementModifiers.FOOD, 0 },
            { ElementModifiers.SCIENCE, 0 },
            { ElementModifiers.WORDS, 0 },
            { ElementModifiers.TEMPERATURE, 0 },
            { ElementModifiers.GRAVITY, 0 },
            { ElementModifiers.PRESSURE, 0 },
            { ElementModifiers.RADIATION, 0 }
        };
    }

    public void ActivateLife()
    {
        _life = new LifeModel();
        _life.Name = "Human";
        _life.Population = 5;
        _life.Science = 1;
        _life.KnownElements = 1;
        _life.WorkingElements = new List<WorkedElementModel>();
        _life.WorkingElements.Add( new WorkedElementModel( 0, 4 ) );
        _life.WorkingElements.Add( new WorkedElementModel( 1, 1 ) );

        _planet.Life = _life;
    }
    
    private void UpdateScience( double value )
    {
        _life.Science += value;
    }

    private void UpdatePopulation( double value )
    {
        _life.Population += value;
    }

    internal void UpdateStep( ulong steps )
    {
        for( uint i = 0; i < steps; i++ )
        {
            SingleUpdate();
        }
    }

    private void SingleUpdate()
    {
        resetUpdateValues();
        for( int i = 0; i < _life.WorkingElements.Count; i++ )
        {
            WorkedElementModel element = _life.WorkingElements[ i ];

            _updateValues[ ElementModifiers.FOOD ] += getTotalWorkersDelta( element, ElementModifiers.FOOD ) - getTotalPopulationFoodConsumption();
            _updateValues[ ElementModifiers.SCIENCE ] += getTotalWorkersDelta( element, ElementModifiers.SCIENCE );

        }

        foreach( KeyValuePair<string, double> item in _updateValues )
        {
            if( item.Value != 0 )
            {
                UpdateLifeProperty( item );
            }
        }

        Debug.Log( _life.Population + "-------------" + _life.Science );
        Log.Add( _life.Population + "," + _life.Science, true );
    }

    private void UpdateLifeProperty( KeyValuePair<string, double> item )
    {
        switch( item.Key )
        {
            case ElementModifiers.FOOD:
                UpdatePopulation( item.Value );
                break;
            case ElementModifiers.SCIENCE:
                UpdateScience( item.Value );
                break;
        }
    }

    private double getTotalPopulationFoodConsumption()
    {
        return ( _life.Science / 100 ) * Math.Floor( _life.Population );
    }

    private double getTotalWorkersDelta( WorkedElementModel element, string name )
    {
        return Config.Elements[ element.Index ].Modifier( name ).Delta * element.Workers;
    }

    private void resetUpdateValues()
    {
        _updateValues[ ElementModifiers.FOOD ] = 0;
        _updateValues[ ElementModifiers.SCIENCE ] = 0;
        _updateValues[ ElementModifiers.WORDS ] = 0;
        _updateValues[ ElementModifiers.TEMPERATURE ] = 0;
        _updateValues[ ElementModifiers.GRAVITY ] = 0;
        _updateValues[ ElementModifiers.PRESSURE ] = 0;
        _updateValues[ ElementModifiers.RADIATION ] = 0;
    }

}
