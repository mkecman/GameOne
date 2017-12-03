using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class PlanetModel
{
    public string Name;
    public LifeModel Life;
    public List<PlanetElementModel> Elements;
    public double Distance;
    public double Radius;
    public double OrbitalPeriod;
    public double Mass;
    public double Volume;
    public double Density;
    public double Gravity;
    public double Temperature;
    public double Pressure;
    public double MagneticField;
    public double AlbedoSurface;
    public double AlbedoClouds;
    public double EscapeVelocity;
}
