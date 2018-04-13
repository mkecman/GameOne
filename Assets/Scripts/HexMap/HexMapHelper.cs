using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HexMapHelper
{
    public static float GetXPosition( int x, int y )
    {
        float xOffset = GameConfig.Get<HexConfig>().xOffset;
        float xPos = x * xOffset;
        if( y % 2 == 1 )
            xPos += xOffset / 2f;
        return xPos;
    }

    public static float GetZPosition( int y )
    {
        return y * GameConfig.Get<HexConfig>().zOffset;
    }

    internal static List<Vector2Int> positions = new List<Vector2Int>();

    internal static List<Vector2Int> GetPositions( int x, int y, int width, int height )
    {
        positions.Clear();
        
        CheckAndAdd( x, y + 1, width, height );

        CheckAndAdd( x - 1, y, width, height );
        CheckAndAdd( x, y, width, height );
        CheckAndAdd( x + 1, y, width, height );

        CheckAndAdd( x, y - 1, width, height );

        if( y % 2 == 0 )
        {
            CheckAndAdd( x - 1, y + 1, width, height );
            CheckAndAdd( x - 1, y - 1, width, height );
        }
        else
        {
            CheckAndAdd( x + 1, y + 1, width, height );
            CheckAndAdd( x + 1, y - 1, width, height );
        }

        return positions;
    }

    internal static void CheckAndAdd( int x, int y, int width, int height )
    {
        if( x >= 0 && y >= 0 && x < width && y < height )
            positions.Add( new Vector2Int(x, y) );
    }
    
}
