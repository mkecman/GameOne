using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;
using System.Linq;

[Serializable]
public class StarModel
{
    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal ReactiveProperty<int> _Index = new ReactiveProperty<int>();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    internal ReactiveProperty<string> _Type = new ReactiveProperty<string>();
    public string Type
    {
        get { return _Type.Value; }
        set { _Type.Value = value; }
    }

    internal ReactiveProperty<string> _Color = new ReactiveProperty<string>();
    public string Color
    {
        get { return _Color.Value; }
        set { _Color.Value = value; }
    }

    internal ReactiveProperty<double> _Temperature = new ReactiveProperty<double>();
    public double Temperature
    {
        get { return _Temperature.Value; }
        set { _Temperature.Value = value; }
    }

    internal ReactiveProperty<double> _Radius = new ReactiveProperty<double>();
    public double Radius
    {
        get { return _Radius.Value; }
        set { _Radius.Value = value; }
    }

    internal ReactiveProperty<double> _Mass = new ReactiveProperty<double>();
    public double Mass
    {
        get { return _Mass.Value; }
        set { _Mass.Value = value; }
    }

    internal ReactiveProperty<double> _Luminosity = new ReactiveProperty<double>();
    public double Luminosity
    {
        get { return _Luminosity.Value; }
        set { _Luminosity.Value = value; }
    }

    internal ReactiveProperty<double> _InnerHabitableZone = new ReactiveProperty<double>();
    public double InnerHabitableZone
    {
        get { return _InnerHabitableZone.Value; }
        set { _InnerHabitableZone.Value = value; }
    }

    internal ReactiveProperty<double> _HabitableZone = new ReactiveProperty<double>();
    public double HabitableZone
    {
        get { return _HabitableZone.Value; }
        set { _HabitableZone.Value = value; }
    }

    internal ReactiveProperty<double> _OuterHabitableZone = new ReactiveProperty<double>();
    public double OuterHabitableZone
    {
        get { return _OuterHabitableZone.Value; }
        set { _OuterHabitableZone.Value = value; }
    }

    internal ReactiveProperty<double> _Lifetime = new ReactiveProperty<double>();
    public double Lifetime
    {
        get { return _Lifetime.Value; }
        set { _Lifetime.Value = value; }
    }

    internal ReactiveProperty<int> _CreatedPlanets = new ReactiveProperty<int>();
    public int CreatedPlanets
    {
        get { return _CreatedPlanets.Value; }
        set { _CreatedPlanets.Value = value; }
    }
    
    internal ReactiveCollection<WeightedValue> _AvailableElements = new ReactiveCollection<WeightedValue>();
    public List<WeightedValue> AvailableElements
    {
        get { return _AvailableElements.ToList<WeightedValue>(); }
        set { _AvailableElements = new ReactiveCollection<WeightedValue>( value ); }
    }

    internal ReactiveCollection<PlanetModel> _Planets = new ReactiveCollection<PlanetModel>();
    public List<PlanetModel> Planets
    {
        get { return _Planets.ToList<PlanetModel>(); }
        set { _Planets = new ReactiveCollection<PlanetModel>( value ); }
    }


}
