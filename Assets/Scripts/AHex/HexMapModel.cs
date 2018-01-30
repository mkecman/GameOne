using UnityEngine;
using System.Collections;

public class HexMapModel
{
    public GridModel heightMap;
    public int Width;
    public int Height;

    public HexMapModel( int width, int height )
    {
        Width = width;
        Height = height;
        heightMap = new GridModel( width, height );
    }
}
