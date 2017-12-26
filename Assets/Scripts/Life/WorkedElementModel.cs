using UnityEngine;
using System.Collections;
using System;
using UniRx;
using LitJson;

[Serializable]
public class WorkedElementModel
{

    public WorkedElementModel(){}

    public WorkedElementModel( int index = 0, int workers = 0 )
    {
        Index = index;
        Workers = workers;
    }

    internal ReactiveProperty<int> _Index = new ReactiveProperty<int>();
    public int Index
    {
        get { return _Index.Value; }
        set { _Index.Value = value; }
    }

    internal ReactiveProperty<int> _Workers = new ReactiveProperty<int>();
    public int Workers
    {
        get { return _Workers.Value; }
        set { _Workers.Value = value; }
    }
}