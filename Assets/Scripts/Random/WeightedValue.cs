using System;
using UniRx;

[Serializable]
public class WeightedValue
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

}
