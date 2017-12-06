using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Galaxy
{
    private GalaxyModel _galaxy;
    private List<Star> _stars;
    
    public void CreateGalaxy( int Index )
    {
        _galaxy = new GalaxyModel();
        _galaxy.Name = "Galaxy " + Index;
        _galaxy.CreatedStars = 0;
        _stars = new List<Star>();
        _galaxy.Stars = new List<StarModel>();
    }

    public void CreateStar( double Words )
    {
        Star star = new Star();
        star.CreateStar( Words, _galaxy.CreatedStars++ );
        _stars.Add( star );
    }
}
