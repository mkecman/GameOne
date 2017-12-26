using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class PlanetElementModel
{
    internal ReactiveProperty<int> _Index = new ReactiveProperty<int>();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }


    internal ReactiveProperty<double> _Amount = new ReactiveProperty<double>();
    public double Amount
    {
        get { return _Amount.Value; }
        set { _Amount.Value = value; }
    }

}
