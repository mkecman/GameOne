using UnityEngine;
using System.Collections;
using UniRx;

public class PlanetProperty
{
    public float Value;
    public float Variation;

    public PlanetProperty( float value, float variation )
    {
        Value = value;
        Variation = variation;
    }
}
