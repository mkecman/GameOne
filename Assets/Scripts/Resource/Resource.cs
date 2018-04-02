﻿using UnityEngine;
using System.Collections;
using UniRx;

public class Resource
{
    public Resource( R type, double value, double delta = 0, Color? color = null )
    {
        Type = type;
        Value = value;
        Delta = delta;
        Color = color ?? Color.black;
    }

    [SerializeField]
    internal R Type = R.Temperature;
    
    [SerializeField]
    internal DoubleReactiveProperty _Value = new DoubleReactiveProperty();
    public double Value
    {
        get { return _Value.Value; }
        set { _Value.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _Delta = new DoubleReactiveProperty();
    public double Delta
    {
        get { return _Delta.Value; }
        set { _Delta.Value = value; }
    }

    [SerializeField]
    internal ReactiveProperty<Color> _Color = new ReactiveProperty<Color>();
    internal Color Color
    {
        get { return _Color.Value; }
        set { _Color.Value = value; }
    }


}

public enum R
{
    Default,
    Altitude,
    Temperature,
    Pressure,
    Humidity,
    Radiation,
    Energy,
    Science,
    Minerals,
    HexScore,
    Population,
    Health,
    Count
}
