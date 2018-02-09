using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Galaxy
{
    private GalaxyModel _model;
    private List<Star> _stars;
    
    public GalaxyModel New( int Index )
    {
        _model = new GalaxyModel
        {
            Name = "Galaxy " + Index,
            CreatedStars = 0,
            Stars = new List<StarModel>()
        };
        _stars = new List<Star>();

        //TODO: Remove this
        NewStar( 7 );

        return _model;
    }

    private void NewStar( int Type )
    {
        Star star = new Star();
        _model._Stars.Add( star.New( Type, _model.CreatedStars ) );
        _stars.Add( star );
    }

    public void Load( GalaxyModel galaxyModel )
    {
        _model = galaxyModel;
        _stars = new List<Star>();
        for( int i = 0; i < _model.Stars.Count; i++ )
        {
            LoadStar( _model.Stars[ i ] );
        }
    }

    private void LoadStar( StarModel starModel )
    {
        Star star = new Star();
        star.Load( starModel );
        _stars.Add( star );
    }
    
    internal void UpdateStep( int steps )
    {
        for( int i = 0; i < _stars.Count; i++ )
        {
            _stars[ i ].UpdateStep( steps );
        }
    }
}
