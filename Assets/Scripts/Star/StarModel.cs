using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class StarModel
{
    public string Name;
    public int Index;
    public string Type;
    public string Color;
    public double Temperature;
    public double Radius;
    public double Mass;
    public double Luminosity;
    public double InnerHabitableZone; 
    public double HabitableZone;
    public double OuterHabitableZone;
    public double Lifetime;
    public List<PlanetModel> Planets;
}
