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
        Props.Add( R.Altitude, new PlanetProperty( 0.5f, 1 ) );
        Props.Add( R.Temperature, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Pressure, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Humidity, new PlanetProperty( 0, 0 ) );
        Props.Add( R.Radiation, new PlanetProperty( 0, 0 ) );
    }

    public LifeModel Life = new LifeModel();
    public GridModel<HexModel> Map;
    public Dictionary<R, PlanetProperty> Props = new Dictionary<R, PlanetProperty>();
    public Dictionary<R, IntReactiveProperty> Impact = new Dictionary<R, IntReactiveProperty>();
    public Dictionary<R, int> Goals = new Dictionary<R, int>();

    internal StringReactiveProperty _Name = new StringReactiveProperty();
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
    
    internal FloatReactiveProperty _Distance = new FloatReactiveProperty();
    public float Distance
    {
        get { return _Distance.Value; }
        set { _Distance.Value = value; }
    }

    internal FloatReactiveProperty _Radius = new FloatReactiveProperty();
    public float Radius
    {
        get { return _Radius.Value; }
        set { _Radius.Value = value; }
    }

    internal FloatReactiveProperty _OrbitalPeriod = new FloatReactiveProperty();
    public float OrbitalPeriod
    {
        get { return _OrbitalPeriod.Value; }
        set { _OrbitalPeriod.Value = value; }
    }

    internal FloatReactiveProperty _Mass = new FloatReactiveProperty();
    public float Mass
    {
        get { return _Mass.Value; }
        set { _Mass.Value = value; }
    }

    internal FloatReactiveProperty _Volume = new FloatReactiveProperty();
    public float Volume
    {
        get { return _Volume.Value; }
        set { _Volume.Value = value; }
    }

    internal FloatReactiveProperty _Density = new FloatReactiveProperty();
    public float Density
    {
        get { return _Density.Value; }
        set { _Density.Value = value; }
    }

    internal FloatReactiveProperty _Gravity = new FloatReactiveProperty();
    public float Gravity
    {
        get { return _Gravity.Value; }
        set { _Gravity.Value = value; }
    }
    
    internal FloatReactiveProperty _Pressure = new FloatReactiveProperty();
    public float Pressure
    {
        get { return _Pressure.Value; }
        set { _Pressure.Value = value; }
    }

    internal FloatReactiveProperty _MagneticField = new FloatReactiveProperty();
    public float MagneticField
    {
        get { return _MagneticField.Value; }
        set { _MagneticField.Value = value; }
    }

    internal FloatReactiveProperty _AlbedoSurface = new FloatReactiveProperty();
    public float AlbedoSurface
    {
        get { return _AlbedoSurface.Value; }
        set { _AlbedoSurface.Value = value; }
    }

    internal FloatReactiveProperty _AlbedoClouds = new FloatReactiveProperty();
    public float AlbedoClouds
    {
        get { return _AlbedoClouds.Value; }
        set { _AlbedoClouds.Value = value; }
    }

    internal FloatReactiveProperty _EscapeVelocity = new FloatReactiveProperty();
    public float EscapeVelocity
    {
        get { return _EscapeVelocity.Value; }
        set { _EscapeVelocity.Value = value; }
    }

    [SerializeField]
    internal FloatReactiveProperty _TotalEnergyValue = new FloatReactiveProperty();
    public float TotalEnergyValue
    {
        get { return _TotalEnergyValue.Value; }
        set { _TotalEnergyValue.Value = value; }
    }


}
