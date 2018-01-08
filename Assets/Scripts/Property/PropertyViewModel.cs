using UnityEngine;
using System.Collections;
using UniRx;

public class PropertyViewModel
{
    public ElementModifiers property;
    public ReactiveProperty<double> value;
    public int propertyLength = 0;
    public string valueFormat = "F3";
    public string deltaFormat = "F2";
    public bool showDelta = false;
}
