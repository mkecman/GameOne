﻿using UnityEngine;
using System.Collections;

public class HexMapHelper
{
    public static float GetXPosition( int x, int y )
    {
        float xOffset = Config.Get<HexConfig>().xOffset;
        float xPos = x * xOffset;
        if( y % 2 == 1 )
            xPos += xOffset / 2f;
        return xPos;
    }

    public static float GetZPosition( int y )
    {
        return y * Config.Get<HexConfig>().zOffset;
    }
}