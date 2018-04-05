using System;
using UniRx;

[Serializable]
public class PlanetElementModel
{
    internal IntReactiveProperty _Index = new IntReactiveProperty();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }


    internal FloatReactiveProperty _Amount = new FloatReactiveProperty();
    public float Amount
    {
        get { return _Amount.Value; }
        set { _Amount.Value = value; }
    }

}
