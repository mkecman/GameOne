﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class LifeModel
{
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }
    
    [SerializeField]
    internal DoubleReactiveProperty _ClimbLevel = new DoubleReactiveProperty();
    public Double ClimbLevel
    {
        get { return _ClimbLevel.Value; }
        set { _ClimbLevel.Value = value; }
    }

    public Dictionary<R, BellCurve> Resistance = new Dictionary<R, BellCurve>();
    public Dictionary<R, Resource> Props = new Dictionary<R, Resource>();

    public ReactiveCollection<UnitModel> Units = new ReactiveCollection<UnitModel>();

    public LifeModel()
    {
        Props.Add( R.Population, new Resource( R.Population, 0 ) );
        Props.Add( R.Energy, new Resource( R.Energy, 0 ) );
        Props.Add( R.Science, new Resource( R.Science, 0 ) );
        Props.Add( R.Minerals, new Resource( R.Minerals, 0 ) );

        Resistance.Add( R.Temperature, new BellCurve( 1, 0.36f, 0.15f ) );
        Resistance.Add( R.Pressure, new BellCurve( 1, 0.63f, 0.15f ) );
        Resistance.Add( R.Humidity, new BellCurve( 1, 1f, 0.3f ) );
        Resistance.Add( R.Radiation, new BellCurve( 1, 0f, 0.3f ) );
    }
}
