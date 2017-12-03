using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StarModel
{
    public string Name;
    public double Mass;
    public double Radius;
    public double Gravity;
    public double Luminosity;
    public double Temperature;
    public double Lifetime;
    public double InnerHabitableZoneDistance;
    public double OuterHabitableZoneDistance;
    public List<PlanetModel> Planets;
}
