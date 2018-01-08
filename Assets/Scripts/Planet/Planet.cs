using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Planet
{
    private PlanetModel _planet;
    private Life _life;
    
    public Planet()
    {
        
    }

    public void Load( PlanetModel planetModel )
    {
        _planet = planetModel;
        if( _planet.Life != null )
        {
            _life = new Life();
            _life.Load( _planet.Life );
        }
    }

    public void ActivateLife()
    {
        _life = new Life();
        _planet.Life = _life.New();
    }

    internal void UpdateStep( int steps )
    {
        for( uint i = 0; i < steps; i++ )
        {
            _life.UpdateStep();
        }
    }
    
}
