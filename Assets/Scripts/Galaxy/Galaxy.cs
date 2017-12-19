using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Galaxy
{
    private GalaxyModel _galaxy;
    private List<Star> _stars;
    
    public void CreateGalaxy( int Index )
    {
        _galaxy = new GalaxyModel();
        _galaxy.Name = "Galaxy " + Index;
        _galaxy.CreatedStars = 0;
        _galaxy.Stars = new List<StarModel>();
        _stars = new List<Star>();
    }

    public void CreateStar( double Words )
    {
        Star star = new Star();
        star.CreateStar( Words, _galaxy.CreatedStars );
        _stars.Add( star );
    }

    public void MoveWorker( int from, int to )
    {
        _stars[ 0 ].MoveWorker( from, to );
    }

    internal void UpdateStep( ulong steps )
    {
        for( int i = 0; i < _stars.Count; i++ )
        {
            _stars[ i ].UpdateStep( steps );
        }
    }
}
