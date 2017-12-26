using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class WeightedValue
{
    internal ReactiveProperty<double> _Weight = new ReactiveProperty<double>();
    public double Weight
    {
        get { return _Weight.Value; }
        set { _Weight.Value = value; }
    }

    internal ReactiveProperty<double> _Value = new ReactiveProperty<double>();
    public double Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

}
