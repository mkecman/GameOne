using UnityEngine;
using System.Collections;

public class GridModel 
{
    public float[,] Table;
    public int Width;
    public int Height;
    public float Min;
    public float Max;

    public GridModel( int width, int height )
    {
        Table = new float[ width, height ];
        Width = width;
        Height = height;
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}
