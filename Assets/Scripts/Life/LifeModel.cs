using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;
using System.Linq;
using LitJson;

[Serializable]
public class LifeModel
{
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }
    
    internal DoubleReactiveProperty _Population = new DoubleReactiveProperty();
    public double Population
    {
        get { return _Population.Value; }
        set { _Population.Value = value; }
    }

    internal DoubleReactiveProperty _Food = new DoubleReactiveProperty();
    public Double Food
    {
        get { return _Food.Value; }
        set { _Food.Value = value; }
    }

    internal DoubleReactiveProperty _Science = new DoubleReactiveProperty();
    public double Science
    {
        get { return _Science.Value; }
        set { _Science.Value = value; }
    }

    internal DoubleReactiveProperty _Words = new DoubleReactiveProperty();
    public double Words
    {
        get { return _Words.Value; }
        set { _Words.Value = value; }
    }

    public BellCurve TemperatureBC = new BellCurve( 1, 0.39f, 0.2f );
    public BellCurve PressureBC = new BellCurve( 1, 0.66f, 0.2f );
    public BellCurve HumidityBC = new BellCurve( 1, .85f, 0.2f );
    public BellCurve RadiationBC = new BellCurve( 1, 0.18f, 0.2f );

    public ReactiveCollection<UnitModel> Units = new ReactiveCollection<UnitModel>();

}
