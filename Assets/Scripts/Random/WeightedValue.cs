using System;

[Serializable]
public class WeightedValue
{
    public float Weight;
    public float Value;

    public WeightedValue( float value, float weight )
    {
        Value = value;
        Weight = weight;
    }
}
