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
    
    public Dictionary<R,BellCurve> Resistance = new Dictionary<R, BellCurve>();
    public Dictionary<R, Resource> Props = new Dictionary<R, Resource>();

    public ReactiveCollection<UnitModel> Units = new ReactiveCollection<UnitModel>();
    public ReactiveCollection<BuildingModel> Buildings = new ReactiveCollection<BuildingModel>();
    public ReactiveDictionary<int, LifeElementModel> Elements = new ReactiveDictionary<int, LifeElementModel>();

    public List<BuildingModel> BuildingsState = new List<BuildingModel>();

    public LifeModel()
    {
        Props.Add( R.Population, new Resource( R.Population, 0 ) );
        Props.Add( R.Energy, new Resource( R.Energy, 0 ) );
        Props.Add( R.Science, new Resource( R.Science, 0 ) );
        Props.Add( R.Minerals, new Resource( R.Minerals, 0 ) );
    }
}
