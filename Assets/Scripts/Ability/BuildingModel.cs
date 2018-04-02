using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class BuildingModel
{
    public int Index;
    public string Name;
    public int UnlockCost;
    public int BuildCost;

    public int X;
    public int Y;
    public double Altitude;

    public Dictionary<R, double> Effects = new Dictionary<R, double>();

    [SerializeField]
    internal ReactiveProperty<BuildingState> _State = new ReactiveProperty<BuildingState>();
    public BuildingState State
    {
        get { return _State.Value; }
        set { _State.Value = value; }
    }

}
