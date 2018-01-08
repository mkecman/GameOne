using System;
using UniRx;
using UnityEngine;

[Serializable]
public class EConnection
{
    public int Index = 0;
    public StringReactiveProperty Formula = new StringReactiveProperty();
    public DoubleReactiveProperty Delta = new DoubleReactiveProperty();

    public StringReactiveProperty SourceName = new StringReactiveProperty();
    public EObject Source;
    public StringReactiveProperty TargetName = new StringReactiveProperty();
    public EObject Target;
}
