using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

[Serializable]
public class ElementModel
{
    
    internal ReactiveProperty<int> _Index = new ReactiveProperty<int>();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    internal StringReactiveProperty _Symbol = new StringReactiveProperty();
    public string Symbol
    {
        get { return _Symbol.Value; }
        set { _Symbol.Value = value; }
    }

    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal FloatReactiveProperty _Weight = new FloatReactiveProperty();
    public float Weight
    {
        get { return _Weight.Value; }
        set { _Weight.Value = value; }
    }

    internal FloatReactiveProperty _Density = new FloatReactiveProperty();
    public float Density
    {
        get { return _Density.Value; }
        set { _Density.Value = value; }
    }

    internal StringReactiveProperty _HexColor = new StringReactiveProperty();
    public string HexColor
    {
        get { return _HexColor.Value; }
        set { _HexColor.Value = value; }
    }

    internal StringReactiveProperty _GroupBlock = new StringReactiveProperty();
    public string GroupBlock
    {
        get { return _GroupBlock.Value; }
        set { _GroupBlock.Value = value; }
    }

    internal ReactiveCollection<ElementModifierModel> _Modifiers = new ReactiveCollection<ElementModifierModel>();
    public List<ElementModifierModel> Modifiers
    {
        get { return _Modifiers.ToList<ElementModifierModel>(); }
        set { _Modifiers = new ReactiveCollection<ElementModifierModel>( value ); }
    }

    public ElementModifierModel Modifier( ElementModifiers Name )
    {
        return _Modifiers[ (int)Name ];
    }
}
