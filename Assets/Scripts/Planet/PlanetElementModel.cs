using System;
using UniRx;
using UnityEngine;

[Serializable]
public class PlanetElementModel
{
    [SerializeField]
    internal IntReactiveProperty _Index = new IntReactiveProperty();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    [SerializeField]
    internal FloatReactiveProperty _Amount = new FloatReactiveProperty();
    public float Amount
    {
        get { return _Amount.Value; }
        set { _Amount.Value = value; }
    }
    
}
