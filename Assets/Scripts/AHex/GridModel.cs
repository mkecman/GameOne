using UnityEngine;
using System.Collections;

public class GridModel<T>
{
    public T[,] Table;
    public int Width;
    public int Height;
    public float Min;
    public float Max;

    public GridModel( int width, int height )
    {
        Table = new T[ width, height ];
        Width = width;
        Height = height;
        Min = float.MaxValue;
        Max = float.MinValue;
    }
}
