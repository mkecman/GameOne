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
        _life.Population = 1;
        _life.Science = 1;
        _life.KnownElements = 2;
        _life.WorkingElements = new List<WorkedElementModel>();
        _life.WorkingElements.Add( new WorkedElementModel( 0, 1 ) );
        _life.WorkingElements.Add( new WorkedElementModel( 1, 0 ) );

        _planet.Life = _life;
    }

    public void MoveWorker( int from, int to )
    {
        if( _life.WorkingElements[ from ].Workers > 0 )
        {
            _life.WorkingElements[ from ].Workers--;
            _life.WorkingElements[ to ].Workers++;
            Debug.Log( "Moved: " + from + " to " + to );
        }
        else
        {
            Debug.Log( "Not Moved!!!" );
        }
        
    }
    
    private void UpdateScience( double value )
    {
        double newScience = _life.Science + value;
        if( newScience > Config.Elements[ _life.KnownElements ].Weight )
        {
            _life.WorkingElements.Add( new WorkedElementModel( _life.KnownElements, 0 ) );
            _life.KnownElements++;
        }
        _life.Science = newScience;
    }

    private void UpdatePopulation( double value )
    {
        double newPopulation = _life.Population + value;

        if( Math.Floor( _life.Population ) < Math.Floor( newPopulation ) )
            _life.WorkingElements[ 0 ].Workers++;

        if( Math.Floor( _life.Population ) > newPopulation )
        {
            for( int i = _life.WorkingElements.Count-1; i >= 0; i-- )
            {
                if( _life.WorkingElements[ i ].Workers > 0 )
                {
                    _life.WorkingElements[ i ].Workers--;
                    break;
                }
            }
        }

        _life.Population = newPopulation;
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
            if( element.Workers > 0 )
            {
                _updateValues[ ElementModifiers.FOOD ] += getTotalWorkersDelta( element, ElementModifiers.FOOD ) - getTotalPopulationFoodConsumption();
                _updateValues[ ElementModifiers.SCIENCE ] += getTotalWorkersDelta( element, ElementModifiers.SCIENCE );
            }
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
        return ( _life.Science / 150 ) * Math.Floor( _life.Population );
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
