using System;
using UniRx;

[Serializable]
public class WeightedValue2
{
    internal FloatReactiveProperty _Weight = new FloatReactiveProperty();
    public float Weight
    {
        get { return _Weight.Value; }
        set { _Weight.Value = value; }
    }

    internal FloatReactiveProperty _Value = new FloatReactiveProperty();
    public float Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

    public WeightedValue2() { }
    public WeightedValue2( float value, float weight )
    {
        Value = value;
        Weight = weight;
    }

}

public struct WeightedValue
{
    public float Weight;
    public float Value;

    public WeightedValue( float value, float weight )
    {
        Value = value;
        Weight = weight;
    }
}
