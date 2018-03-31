using UnityEngine;
using System.Collections;

public class BuildingMessage
{
    public BuildingState State;
    public int Index;
    public int X;
    public int Y;

    public BuildingMessage( BuildingState state, int index )
    {
        State = state;
        Index = index;
    }
}
