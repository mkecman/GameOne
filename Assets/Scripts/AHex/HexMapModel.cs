﻿using UnityEngine;
using System.Collections;

public class HexMapModel
{
    public GridModel<float> heightMap;
    public GridModel<Color> colorMap;
    public int Width;
    public int Height;

    public HexMapModel( int width, int height )
    {
        Width = width;
        Height = height;
        heightMap = new GridModel<float>( width, height );
        colorMap = new GridModel<Color>( width, height );
    }
}