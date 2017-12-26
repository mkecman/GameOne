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
    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal ReactiveCollection<WorkedElementModel> _WorkingElements = new ReactiveCollection<WorkedElementModel>();
    public List<WorkedElementModel> WorkingElements
    {
        get { return _WorkingElements.ToList<WorkedElementModel>(); }
        set { _WorkingElements = new ReactiveCollection<WorkedElementModel>( value ); }
    }

    internal ReactiveProperty<double> _Population = new ReactiveProperty<double>();
    public double Population
    {
        get { return _Population.Value; }
        set { _Population.Value = value; }
    }

    internal ReactiveProperty<double> _Science = new ReactiveProperty<double>();
    public double Science
    {
        get { return _Science.Value; }
        set { _Science.Value = value; }
    }

    internal ReactiveProperty<int> _KnownElements = new ReactiveProperty<int>();
    public int KnownElements
    {
        get { return _KnownElements.Value; }
        set { _KnownElements.Value = value; }
    }

    
}
