using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StarsConfig
{
    public int MinPlanets;
    public int MaxPlanets;
    public List<double> FivePlanetDistancesInAU;
    public List<double> TenPlanetDistancesInAU;
    public double MinPlanetaryRadiusInMeters;
    public List<WeightedValue> PlanetaryRadiusInMeters;
    public double MaxElementsBellCurveMagnifier;
    public List<StarModel> Stars;


}
