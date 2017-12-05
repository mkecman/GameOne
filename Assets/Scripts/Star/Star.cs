using UnityEngine;
using System.Collections;

public class Star
{
    private StarModel _star;
    
    public void CreateStar( double Words, int Index )
    {
        _star = Config.Stars.Get( (int)Words );
        _star.Name = "Star " + Index;

        generatePlanets();
    }

    private void generatePlanets()
    {
        int planetCount = Random.Range( Config.Universe.universe.MinPlanets, Config.Universe.universe.MaxPlanets );
        for( int index = 0; index < planetCount; index++ )
        {
            PlanetModel tempPlanet = new PlanetModel();

        }
    }
}
