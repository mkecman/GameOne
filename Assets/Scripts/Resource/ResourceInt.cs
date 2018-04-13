using UnityEngine;
using System.Collections;
using UniRx;

public class ResourceInt
{
    public ResourceInt( S type, int value, int delta = 0, int minValue = 0, int maxValue = 1, Color? color = null )
    {
        Type = type;
        MinValue = minValue;
        MaxValue = maxValue;
        Value = value;
        Delta = delta;
        Color = color ?? Color.black;
    }

    [SerializeField]
    internal S Type = S.HP;
    
    [SerializeField]
    internal IntReactiveProperty _Value = new IntReactiveProperty();
    public int Value
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
    internal IntReactiveProperty _Delta = new IntReactiveProperty();
    public int Delta
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
    internal IntReactiveProperty _MinValue = new IntReactiveProperty();
    public int MinValue
    {
        get { return _MinValue.Value; }
        set { _MinValue.Value = value; }
    }

    [SerializeField]
    internal IntReactiveProperty _MaxValue = new IntReactiveProperty();
    public int MaxValue
    {
        get { return _MaxValue.Value; }
        set { _MaxValue.Value = value; }
    }



}

public enum S
{
    HP,
    XP,
    STR,
    DEX,
    LUK,
    ATK,
    Count
}
