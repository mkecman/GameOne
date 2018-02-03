using UnityEngine;
using System.Collections;

public class UnitMoveMessage
{
    public int X;
    public int Y;
    public UnitMoveMessage( int xTo, int yTo )
    {
        this.X = xTo;
        this.Y = yTo;
    }
}
