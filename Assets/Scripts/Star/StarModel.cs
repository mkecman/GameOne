using System;
using UniRx;

[Serializable]
public class StarModel
{
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal IntReactiveProperty _Index = new IntReactiveProperty();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    internal StringReactiveProperty _Type = new StringReactiveProperty();
    public string Type
    {
        get { return _Type.Value; }
        set { _Type.Value = value; }
    }

    internal StringReactiveProperty _Color = new StringReactiveProperty();
    public string Color
    {
        get { return _Color.Value; }
        set { _Color.Value = value; }
    }

    internal FloatReactiveProperty _Temperature = new FloatReactiveProperty();
    public float Temperature
    {
        get { return _Temperature.Value; }
        set { _Temperature.Value = value; }
    }

    internal FloatReactiveProperty _Radius = new FloatReactiveProperty();
    public float Radius
    {
        get { return _Radius.Value; }
        set { _Radius.Value = value; }
    }

    internal FloatReactiveProperty _Mass = new FloatReactiveProperty();
    public float Mass
    {
        get { return _Mass.Value; }
        set { _Mass.Value = value; }
    }

    internal FloatReactiveProperty _Luminosity = new FloatReactiveProperty();
    public float Luminosity
    {
        get { return _Luminosity.Value; }
        set { _Luminosity.Value = value; }
    }

    internal FloatReactiveProperty _InnerHabitableZone = new FloatReactiveProperty();
    public float InnerHabitableZone
    {
        get { return _InnerHabitableZone.Value; }
        set { _InnerHabitableZone.Value = value; }
    }

    internal FloatReactiveProperty _HabitableZone = new FloatReactiveProperty();
    public float HabitableZone
    {
        get { return _HabitableZone.Value; }
        set { _HabitableZone.Value = value; }
    }

    internal FloatReactiveProperty _OuterHabitableZone = new FloatReactiveProperty();
    public float OuterHabitableZone
    {
        get { return _OuterHabitableZone.Value; }
        set { _OuterHabitableZone.Value = value; }
    }

    internal FloatReactiveProperty _Lifetime = new FloatReactiveProperty();
    public float Lifetime
    {
        get { return _Lifetime.Value; }
        set { _Lifetime.Value = value; }
    }

    internal ReactiveProperty<int> _PlanetsCount = new ReactiveProperty<int>();
    public int PlanetsCount
    {
        get { return _PlanetsCount.Value; }
        set { _PlanetsCount.Value = value; }
    }

    public ReactiveCollection<WeightedValue> _AvailableElements = new ReactiveCollection<WeightedValue>();

    public ReactiveCollection<PlanetModel> _Planets = new ReactiveCollection<PlanetModel>();



}
