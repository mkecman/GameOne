using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StarsConfig
{
    public int MinPlanets;
    public int MaxPlanets;
    public List<float> FivePlanetDistancesInAU;
    public List<float> TenPlanetDistancesInAU;
    public float MinPlanetaryRadiusInMeters;
    public List<WeightedValue> PlanetaryRadiusInMeters;
    public float MaxElementsBellCurveMagnifier;
    public List<StarModel> Stars;


}
