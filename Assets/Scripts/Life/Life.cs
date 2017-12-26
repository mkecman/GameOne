using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Life 
{
    private LifeModel _life;
    private Dictionary<string, double> _updateValues;
    private List<ElementModel> _elements;

    public Life()
    {
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

        _elements = Config.Get<ElementConfig>().Elements;
    }
    
    public LifeModel New()
    {
        _life = new LifeModel
        {
            Name = "Human",
            Population = 1,
            Science = 4,
            KnownElements = 2,
            WorkingElements = new List<WorkedElementModel>
            {
                new WorkedElementModel( 1, 1 )
            }
        };

        GameModel.Register<LifeModel>( _life );
        
        return _life;
    }

    internal void Load( LifeModel life )
    {
        _life = life;
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
        double elementWeight = _elements[ _life.KnownElements ].Weight;
        if( newScience > elementWeight )
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
            for( int i = _life.WorkingElements.Count - 1; i >= 0; i-- )
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

    public void UpdateStep()
    {
        ResetUpdateValues();
        for( int i = 0; i < _life.WorkingElements.Count; i++ )
        {
            WorkedElementModel element = _life.WorkingElements[ i ];
            if( element.Workers > 0 )
            {
                _updateValues[ ElementModifiers.FOOD ] += GetTotalWorkersDelta( element, ElementModifiers.FOOD ) - GetTotalPopulationFoodConsumption();
                _updateValues[ ElementModifiers.SCIENCE ] += GetTotalWorkersDelta( element, ElementModifiers.SCIENCE );
            }
        }

        foreach( KeyValuePair<string, double> item in _updateValues )
        {
            if( item.Value != 0 )
            {
                UpdateLifeProperty( item );
            }
        }

        CSV.Add( _life.Population + "," + _life.Science );
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

    private double GetTotalPopulationFoodConsumption()
    {
        return ( _life.Science / 500 ) * Math.Floor( _life.Population );
    }

    private double GetTotalWorkersDelta( WorkedElementModel element, string name )
    {
        return _elements[ element.Index ].Modifier( name ).Delta * element.Workers;
    }

    private void ResetUpdateValues()
    {
        _updateValues[ ElementModifiers.FOOD ] = 0;
        _updateValues[ ElementModifiers.SCIENCE ] = 0;
        _updateValues[ ElementModifiers.WORDS ] = 0;
        _updateValues[ ElementModifiers.TEMPERATURE ] = 0;
        _updateValues[ ElementModifiers.GRAVITY ] = 0;
        _updateValues[ ElementModifiers.PRESSURE ] = 0;
        _updateValues[ ElementModifiers.RADIATION ] = 0;
    }
    
    public LifeModel Model
    {
        get { return _life; }
    }
}
