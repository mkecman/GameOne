using UnityEngine;
using System.Collections;
using UniRx;

public class Resource
{
    public Resource( R type, float value, float delta = 0, float minValue = 0, float maxValue = 1, Color? color = null )
    {
        Type = type;
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
        Delta = delta;
        Color = color ?? Color.black;
    }

    [SerializeField]
    internal R Type = R.Temperature;
    
    [SerializeField]
    internal FloatReactiveProperty _Value = new FloatReactiveProperty();
    public float Value
    {
        get { return _Value.Value; }
        set
        {
            if( value > MaxValue )
                value = MaxValue;
            else
            if( value < MinValue )
                value = MinValue;

            _Value.Value = value;
        }
    }

    [SerializeField]
    internal FloatReactiveProperty _Delta = new FloatReactiveProperty();
    public float Delta
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

    [SerializeField]
    internal FloatReactiveProperty _MinValue = new FloatReactiveProperty();
    public float MinValue
    {
        get { return _MinValue.Value; }
        set { _MinValue.Value = value; }
    }

    [SerializeField]
    internal FloatReactiveProperty _MaxValue = new FloatReactiveProperty();
    public float MaxValue
    {
        get { return _MaxValue.Value; }
        set { _MaxValue.Value = value; }
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
    Element,
    Experience,
    Body,
    Speed,
    Mind,
    Soul,
    Armor,
    Attack,
    UpgradePoint,
    Level,
    Critical,
    Count
}
