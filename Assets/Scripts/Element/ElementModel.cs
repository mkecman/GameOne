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

    internal ReactiveProperty<string> _Symbol = new ReactiveProperty<string>();
    public string Symbol
    {
        get { return _Symbol.Value; }
        set { _Symbol.Value = value; }
    }

    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal ReactiveProperty<double> _Weight = new ReactiveProperty<double>();
    public double Weight
    {
        get { return _Weight.Value; }
        set { _Weight.Value = value; }
    }

    internal ReactiveProperty<double> _Density = new ReactiveProperty<double>();
    public double Density
    {
        get { return _Density.Value; }
        set { _Density.Value = value; }
    }

    internal ReactiveProperty<string> _HexColor = new ReactiveProperty<string>();
    public string HexColor
    {
        get { return _HexColor.Value; }
        set { _HexColor.Value = value; }
    }

    internal ReactiveProperty<string> _GroupBlock = new ReactiveProperty<string>();
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
