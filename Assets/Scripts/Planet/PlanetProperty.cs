using UnityEngine;
using System.Collections;
using UniRx;

public class PlanetProperty
{
    public double Value;
    public double Variation;

    public PlanetProperty( double value, double variation )
    {
        Value = value;
        Variation = variation;
    }
}
