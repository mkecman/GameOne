using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class BodySlotModel
{
    public int Index;

    [SerializeField]
    internal BoolReactiveProperty _IsEnabled = new BoolReactiveProperty( false );
    public bool IsEnabled
    {
        get { return _IsEnabled.Value; }
        set { _IsEnabled.Value = value; }
    }

    [SerializeField]
    internal IntReactiveProperty _CompoundIndex = new IntReactiveProperty( Int32.MaxValue );
    public int CompoundIndex
    {
        get { return _CompoundIndex.Value; }
        set { _CompoundIndex.Value = value; }
    }

    public BodySlotModel( int index, bool isEnabled )
    {
        Index = index;
        IsEnabled = isEnabled;
    }
}
