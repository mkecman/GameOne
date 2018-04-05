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
    public float Altitude;

    public Dictionary<R, float> Effects = new Dictionary<R, float>();

    [SerializeField]
    internal ReactiveProperty<BuildingState> _State = new ReactiveProperty<BuildingState>();
    public BuildingState State
    {
        get { return _State.Value; }
        set { _State.Value = value; }
    }

}
