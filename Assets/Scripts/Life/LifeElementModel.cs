using System;
using UniRx;
using UnityEngine;

[Serializable]
public class LifeElementModel
{
    public LifeElementModel() { }

    public LifeElementModel( int index, string symbol, int amount, int maxAmount = 100 )
    {
        Index = index;
        Symbol = symbol;
        MaxAmount = maxAmount;
        Amount = amount;
    }

    [SerializeField]
    internal IntReactiveProperty _Index = new IntReactiveProperty();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    [SerializeField]
    internal StringReactiveProperty _Symbol = new StringReactiveProperty();
    public string Symbol
    {
        get { return _Symbol.Value; }
        set { _Symbol.Value = value; }
    }


    [SerializeField]
    internal IntReactiveProperty _Amount = new IntReactiveProperty();
    public int Amount
    {
        get { return _Amount.Value; }
        set
        {
            if( value > MaxAmount )
                value = MaxAmount;

            _Amount.Value = value;
        }
    }

    [SerializeField]
    internal IntReactiveProperty _MaxAmount = new IntReactiveProperty();
    public int MaxAmount
    {
        get { return _MaxAmount.Value; }
        set { _MaxAmount.Value = value; }
    }


}
