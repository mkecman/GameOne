using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star
{
    private StarModel _star;
    private List<PlanetModel> _planets;
    
    public void CreateStar( double Words, int Index )
    {
        _star = Config.Stars.Get( (int)Words );
        _star.Name = "Star " + Index;

        GeneratePlanets( Words );
    }

    private void GeneratePlanets( double Words )
    {
        int planetCount = Random.Range( Config.Universe.Data.MinPlanets, Config.Universe.Data.MaxPlanets );
        for( int index = 0; index < planetCount; index++ )
        {
            PlanetModel tempPlanet = new PlanetModel();

        }
    }
}
