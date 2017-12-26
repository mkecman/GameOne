using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;
using System;
using System.Collections.Generic;

[Serializable]
public class ElementModifierModel
{
    internal ReactiveProperty<string> _Property = new ReactiveProperty<string>();
    public string Property
    {
        get { return _Property.Value; }
        set { _Property.Value = value; }
    }

    internal ReactiveProperty<double> _Delta = new ReactiveProperty<double>();
    public double Delta
    {
        get { return _Delta.Value; }
        set { _Delta.Value = value; }
    }


}
