using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Galaxy
{
    private GalaxyModel _galaxy;
    private List<Star> _stars;

    public Galaxy()
    {
    }
    
    public GalaxyModel New( int Index )
    {
        _galaxy = new GalaxyModel
        {
            Name = "Galaxy " + Index,
            CreatedStars = 0,
            Stars = new List<StarModel>()
        };
        _stars = new List<Star>();
        return _galaxy;
    }

    public void Load( GalaxyModel galaxyModel )
    {
        _galaxy = galaxyModel;
        _stars = new List<Star>();
        for( int i = 0; i < _galaxy.Stars.Count; i++ )
        {
            LoadStar( _galaxy.Stars[ i ] );
        }
    }

    private void LoadStar( StarModel starModel )
    {
        Star star = new Star();
        star.Load( starModel );
        _stars.Add( star );
    }

    public void NewStar( int Type )
    {
        Star star = new Star();
        _galaxy.Stars.Add( star.New( Type, _galaxy.CreatedStars ) );
        _stars.Add( star );
    }
    
    internal void UpdateStep( ulong steps )
    {
        for( int i = 0; i < _stars.Count; i++ )
        {
            _stars[ i ].UpdateStep( steps );
        }
    }
}
