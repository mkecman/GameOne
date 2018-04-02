using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;
using System.Linq;

[Serializable]
public class PlanetModel
{
    public PlanetModel()
    {
        Props.Add( R.Altitude, new PlanetProperty( 0.5, 1 ) );
        Props.Add( R.Temperature, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Pressure, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Humidity, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Radiation, new PlanetProperty( 0, 0 ) );
    }

    public LifeModel Life;
    public GridModel<HexModel> Map;

    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    [SerializeField]
    internal IntReactiveProperty _Index = new IntReactiveProperty();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    public Dictionary<R,PlanetProperty> Props = new Dictionary<R,PlanetProperty>();

    public ReactiveCollection<PlanetElementModel> _Elements = new ReactiveCollection<PlanetElementModel>();
    
    internal ReactiveProperty<double> _Distance = new ReactiveProperty<double>();
    public double Distance
    {
        get { return _Distance.Value; }
        set { _Distance.Value = value; }
    }

    internal ReactiveProperty<double> _Radius = new ReactiveProperty<double>();
    public double Radius
    {
        get { return _Radius.Value; }
        set { _Radius.Value = value; }
    }

    internal ReactiveProperty<double> _OrbitalPeriod = new ReactiveProperty<double>();
    public double OrbitalPeriod
    {
        get { return _OrbitalPeriod.Value; }
        set { _OrbitalPeriod.Value = value; }
    }

    internal ReactiveProperty<double> _Mass = new ReactiveProperty<double>();
    public double Mass
    {
        get { return _Mass.Value; }
        set { _Mass.Value = value; }
    }

    internal ReactiveProperty<double> _Volume = new ReactiveProperty<double>();
    public double Volume
    {
        get { return _Volume.Value; }
        set { _Volume.Value = value; }
    }

    internal ReactiveProperty<double> _Density = new ReactiveProperty<double>();
    public double Density
    {
        get { return _Density.Value; }
        set { _Density.Value = value; }
    }

    internal ReactiveProperty<double> _Gravity = new ReactiveProperty<double>();
    public double Gravity
    {
        get { return _Gravity.Value; }
        set { _Gravity.Value = value; }
    }
    
    internal ReactiveProperty<double> _Pressure = new ReactiveProperty<double>();
    public double Pressure
    {
        get { return _Pressure.Value; }
        set { _Pressure.Value = value; }
    }

    internal ReactiveProperty<double> _MagneticField = new ReactiveProperty<double>();
    public double MagneticField
    {
        get { return _MagneticField.Value; }
        set { _MagneticField.Value = value; }
    }

    internal ReactiveProperty<double> _AlbedoSurface = new ReactiveProperty<double>();
    public double AlbedoSurface
    {
        get { return _AlbedoSurface.Value; }
        set { _AlbedoSurface.Value = value; }
    }

    internal ReactiveProperty<double> _AlbedoClouds = new ReactiveProperty<double>();
    public double AlbedoClouds
    {
        get { return _AlbedoClouds.Value; }
        set { _AlbedoClouds.Value = value; }
    }

    internal ReactiveProperty<double> _EscapeVelocity = new ReactiveProperty<double>();
    public double EscapeVelocity
    {
        get { return _EscapeVelocity.Value; }
        set { _EscapeVelocity.Value = value; }
    }

    [SerializeField]
    internal DoubleReactiveProperty _LiquidLevel = new DoubleReactiveProperty();
    public double LiquidLevel
    {
        get { return _LiquidLevel.Value; }
        set { _LiquidLevel.Value = value; }
    }


    [SerializeField]
    internal DoubleReactiveProperty _TotalEnergyValue = new DoubleReactiveProperty();
    public double TotalEnergyValue
    {
        get { return _TotalEnergyValue.Value; }
        set { _TotalEnergyValue.Value = value; }
    }


}
