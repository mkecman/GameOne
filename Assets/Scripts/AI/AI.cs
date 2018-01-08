using System;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private PlayerModel _player;
    private LifeModel _life;
    private List<ElementModel> _elements;
    private double _lastPopulation;

    private ElementModifierModel _bestFood;
    private ElementModifierModel _worstFood;
    private int _bestFoodIndex;
    private int _worstFoodIndex;

    private ElementModifierModel _bestScience;
    private ElementModifierModel _worstScience;
    private int _bestScienceIndex;
    private int _worstScienceIndex;

    internal void SetPlayer( PlayerModel playerModel )
    {
        _player = playerModel;
        _life = _player._Galaxies[ 0 ]._Stars[ 0 ]._Planets[ 0 ].Life;
        _elements = Config.Get<ElementConfig>().Elements;
        resetValues();
    }

    private void resetValues()
    {
        _worstFood = new ElementModifierModel();
        _worstFood.Delta = 1;
        _bestFood = new ElementModifierModel();

        _worstScience = new ElementModifierModel();
        _worstScience.Delta = 1;
        _bestScience = new ElementModifierModel();

        _bestScienceIndex = 0;
        _bestFoodIndex = 0;
    }

    internal void MakeMove()
    {
        if( _life.Population < _lastPopulation )
        {
            for( int j = 0; j < 10; j++ )
            {

                resetValues();
                for( int i = 1; i < _life._WorkingElements.Count; i++ )
                {
                    if( _life._WorkingElements[ i ].Workers < 10 )
                    {
                        if( _elements[ i ].Modifier( ElementModifiers.Food ).Delta > _bestFood.Delta )
                        {
                            if( _elements[ i ].Modifier( ElementModifiers.Science ).Delta <= _worstScience.Delta )
                            {
                                _bestFood = _elements[ i ].Modifier( ElementModifiers.Food );
                                _bestFoodIndex = i;
                                _worstScience = _elements[ i ].Modifier( ElementModifiers.Science );
                                _worstScienceIndex = i;
                            }
                        }
                    }





                    if( _life._WorkingElements[ i ].Workers > 0 )
                    {
                        if( _elements[ i ].Modifier( ElementModifiers.Food ).Delta < _worstFood.Delta )
                        {
                            _worstFood = _elements[ i ].Modifier( ElementModifiers.Food );
                            _worstFoodIndex = i;
                        }
                    }

                    if( _life._WorkingElements[ i ].Workers > 0 )
                    {
                        if( _elements[ i ].Modifier( ElementModifiers.Science ).Delta > _bestScience.Delta )
                        {
                            _bestScience = _elements[ i ].Modifier( ElementModifiers.Science );
                            _bestScienceIndex = i;
                        }
                    }
                    
                }

                if( _bestScienceIndex != _bestFoodIndex && _bestScienceIndex != 0 && _bestFoodIndex != 0 )
                {
                    WorkerMoveMessage message = new WorkerMoveMessage();
                    message.From = _bestScienceIndex;
                    message.To = _bestFoodIndex;
                    Debug.Log( message.From + " => " + message.To );
                    GameMessage.Send<WorkerMoveMessage>( message );
                    
                }

                /*
                if( _worstFoodIndex != _bestFoodIndex )
                {
                    WorkerMoveMessage message = new WorkerMoveMessage();
                    message.From = _worstFoodIndex;
                    message.To = _bestFoodIndex;
                    Debug.Log( message.From + " => " + message.To );
                    GameMessage.Send<WorkerMoveMessage>( message );
                }
                if( _worstScienceIndex != _bestScienceIndex )
                {
                    WorkerMoveMessage message2 = new WorkerMoveMessage();
                    message2.From = _bestScienceIndex;
                    message2.To = _worstScienceIndex;
                    Debug.Log( "Science: " + message2.From + " => " + message2.To );
                    GameMessage.Send<WorkerMoveMessage>( message2 );
                }
                */
            }
        }
        _lastPopulation = _life.Population;
    }
}
