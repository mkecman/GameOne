using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Life 
{
    private LifeModel _life;
    private List<ElementModel> _elements;

    private double[] _updateValues;
    private Action<double>[] _actions;

    public Life()
    {
        _elements = Config.Get<ElementConfig>().Elements;

        _updateValues = new double[ (int)ElementModifiers.Count ];
        _actions = new Action<double>[ (int)ElementModifiers.Count ]
        {
            UpdatePopulation, UpdateScience, UpdateWords, UpdateDummy, UpdateDummy, UpdateDummy, UpdateDummy
        };

        GameMessage.Listen<WorkerMoveMessage>( OnWorkerMove );
    }

    private void OnWorkerMove( WorkerMoveMessage value )
    {
        MoveWorker( value.From, value.To );
    }

    public LifeModel New()
    {
        _life = new LifeModel
        {
            Name = "Human",
            Population = 1,
            Science = 4,
            NextElement = 3,
            WorkingElements = new List<WorkedElementModel>
            {
                new WorkedElementModel( 1, 1 ),
                new WorkedElementModel( 2, 0 )
            }
        };

        GameModel.Register( _life );
        
        return _life;
    }

    internal void Load( LifeModel life )
    {
        _life = life;
    }

    public void MoveWorker( int from, int to )
    {
        if( _life._WorkingElements[ from ].Workers > 0 && _life._WorkingElements[ to ].Workers < 10 )
        {
            _life._WorkingElements[ from ].Workers--;
            _life._WorkingElements[ to ].Workers++;
            Debug.Log( "Moved: " + from + " to " + to );
        }
        else
        {
            Debug.Log( "Not Moved!!!" );
        }
    }

    private void UpdateWords( double value )
    {
        _life.Words += value;
    }

    private void UpdateScience( double value )
    {
        if( _life.NextElement >= _elements.Count )
            return;

        double newScience = _life.Science + ( value );
        double elementWeight = _elements[ _life.NextElement ].Weight;
        if( newScience > elementWeight )
        {
            _life._WorkingElements.Add( _life.NextElement, new WorkedElementModel( _life.NextElement, 0 ) );
            _life.NextElement++;
        }
        
        _life.Science = newScience;
    }

    private void UpdatePopulation( double value )
    {
        double newPopulation = _life.Population + value - GetTotalPopulationFoodConsumption();
        int difference = (int)( Math.Floor( newPopulation ) - Math.Floor( _life.Population ) );

        for( int i = 1; i <= _life._WorkingElements.Count; i++ )
        {
            while( difference > 0 && _life._WorkingElements[ i ].Workers < 10 )
            {
                _life._WorkingElements[ i ].Workers++;
                difference--;
            }
        }

        for( int i = _life._WorkingElements.Count; i > 0; i-- )
        {
            while( difference < 0 && _life._WorkingElements[ i ].Workers > 0 )
            {
                _life._WorkingElements[ i ].Workers--;
                difference++;
            }
        }

        _life.Population = newPopulation;
    }

    private void UpdateDummy( double value )
    {

    }

    public void UpdateStep()
    {
        ResetUpdateValues();
        foreach( KeyValuePair<int, WorkedElementModel>item in _life._WorkingElements )
        {
            if( item.Value.Workers > 0 )
            {
                for( int i = 0; i < (int)ElementModifiers.Count; i++ )
                {
                    _updateValues[ i ] += GetTotalWorkersDelta( item.Value, i );
                }
            }
        }

        for( int i = 0; i < _updateValues.Length; i++ )
        {
            _actions[ i ]( _updateValues[ i ] );
        }
        
        CSV.Add( _life.Population + "," + _life.Science + "," + _life.Words );
    }
    
    private double GetTotalPopulationFoodConsumption()
    {
        return ( _life.Science * 0.001 ) * Math.Floor( _life.Population );
    }

    private double GetTotalWorkersDelta( WorkedElementModel element, int index )
    {
        return _elements[ element.Index ]._Modifiers[ index ].Delta * element.Workers;
    }

    private void ResetUpdateValues()
    {
        Array.Clear( _updateValues, 0, _updateValues.Length );
    }
    
    public LifeModel Model
    {
        get { return _life; }
    }
}
