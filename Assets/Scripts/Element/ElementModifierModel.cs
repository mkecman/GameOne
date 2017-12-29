using UnityEngine;
using System.Collections;
using UniRx;
using System.Linq;
using System;
using System.Collections.Generic;

[Serializable]
public class ElementModifierModel
{
    internal ReactiveProperty<ElementModifiers> _Property = new ReactiveProperty<ElementModifiers>();
    public ElementModifiers Property
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
