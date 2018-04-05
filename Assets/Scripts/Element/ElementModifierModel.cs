using System;
using UniRx;

[Serializable]
public class ElementModifierModel
{
    internal ReactiveProperty<ElementModifiers> _Property = new ReactiveProperty<ElementModifiers>();
    public ElementModifiers Property
    {
        get { return _Property.Value; }
        set { _Property.Value = value; }
    }

    internal FloatReactiveProperty _Delta = new FloatReactiveProperty();
    public float Delta
    {
        get { return _Delta.Value; }
        set { _Delta.Value = value; }
    }


}
