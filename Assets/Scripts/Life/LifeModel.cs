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

    private List<WorkedElementModel> _WorkingElementsList;
    internal ReactiveDictionary<int, WorkedElementModel> _WorkingElements = new ReactiveDictionary<int, WorkedElementModel>();
    public List<WorkedElementModel> WorkingElements
    {
        get { return _WorkingElementsList; }
        set { {
                _WorkingElements = new ReactiveDictionary<int, WorkedElementModel>( value.ToDictionary( WorkingElementKeySelector ) );
                _WorkingElementsList = new List<WorkedElementModel>( _WorkingElements.Values );
            } }
    }

    private int WorkingElementKeySelector( WorkedElementModel arg )
    {
        return arg.Index;
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

    internal ReactiveProperty<int> _NextElement = new ReactiveProperty<int>();
    public int NextElement
    {
        get { return _NextElement.Value; }
        set { _NextElement.Value = value; }
    }

    
}
