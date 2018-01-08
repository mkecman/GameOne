using System;
using UniRx;
using UnityEngine;

[Serializable]
public class EConnection
{
    public int Index = 0;
    public StringReactiveProperty Formula = new StringReactiveProperty();
    public StringReactiveProperty SourceName = new StringReactiveProperty();
    public StringReactiveProperty TargetName = new StringReactiveProperty();
    public DoubleReactiveProperty Delta = new DoubleReactiveProperty();
    public ObjectComponent Target;
}
