using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class Star
{
    private StarModel _star;
    
    public void CreateStar( double Words, int Index )
    {
        _star = Config.Stars.Get( (int)Words );
        _star.Name = "Star " + Index;
        _star.Planets = new List<PlanetModel>();
        GeneratePlanets( Words );
    }

    private void GeneratePlanets( double Words )
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("------------------------");
        sb.AppendLine();

        int planetCount = RandomUtil.FromRangeInt( Config.Stars.Settings.MinPlanets, Config.Stars.Settings.MaxPlanets );
        sb.Append("planetCount:" + planetCount);
        sb.AppendLine();


        for ( int index = 0; index < planetCount; index++ )
        {
            PlanetModel tempPlanet = new PlanetModel();
            tempPlanet.Name = "Planet" + _star.CreatedPlanets++;
            tempPlanet.Distance = GetDistance(index, planetCount);
            tempPlanet.Radius = GetRadius();
            tempPlanet.OrbitalPeriod = 2*Math.PI * Math.Sqrt( Math.Pow( tempPlanet.Distance, 3) / _star.Mass * Config.Universe.Constants.G );
            tempPlanet.Mass = (4 * Math.PI * Math.Pow(tempPlanet.Radius, 3)) / Math.Pow(tempPlanet.OrbitalPeriod, 2) * Config.Universe.Constants.G;
            tempPlanet.Volume = 1.333*Math.PI * Math.Pow( tempPlanet.Radius, 3); 
            tempPlanet.Density = tempPlanet.Mass / tempPlanet.Volume;
            tempPlanet.Gravity = ( Config.Universe.Constants.G * tempPlanet.Mass ) / Math.Pow( tempPlanet.Radius, 2 );
            tempPlanet.Temperature = 1;
            tempPlanet.Pressure = 1;
            tempPlanet.MagneticField = 1;
            tempPlanet.AlbedoSurface = 1;
            tempPlanet.AlbedoClouds = 1;
            tempPlanet.EscapeVelocity = Math.Sqrt( ( 2*Config.Universe.Constants.G * tempPlanet.Mass ) / tempPlanet.Radius );

            _star.Planets.Add(tempPlanet);
        }

        Debug.Log("done generating planets");
        Debug.Log(sb);
    }

    private double GetRadius()
    {
        int key = RandomUtil.GetWeightedKey(Config.Stars.Settings.PlanetaryRadiusInMeters);
        double minRadius = Config.Stars.Settings.PlanetaryRadiusInMeters[key].Value;
        double maxRadius = Config.Stars.Settings.PlanetaryRadiusInMeters[key+1].Value;
        
        return RandomUtil.FromRange(minRadius, maxRadius);
    }

    private double GetDistance(int index, int planetCount)
    {
        if (planetCount > 5)
            return Config.Stars.Settings.TenPlanetDistancesInAU[index] * Config.Universe.Constants.AUInMeters;
        else
            return Config.Stars.Settings.FivePlanetDistancesInAU[index] * Config.Universe.Constants.AUInMeters;
    }
}
