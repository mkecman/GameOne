using UnityEngine;
using System.Collections;

public class UnitMessage
{
    public UnitMessageType Type;
    public int X, Y;
    public UnitMessage( UnitMessageType type, int x, int y )
    {
        Type = type;
        X = x;
        Y = y;
    }
}

public enum UnitMessageType
{
    Add,
    Move
}
