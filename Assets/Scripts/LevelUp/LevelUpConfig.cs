using System.Collections.Generic;
using UnityEngine;

public class LevelUpConfig : List<LevelUpModel>
{
    private readonly int DivisorMultiplier = 3;

    public int GetLevelFromXP( int xp )
    {
        return Mathf.CeilToInt( xp / ( Mathf.Sqrt( xp ) * DivisorMultiplier ) );
    }
}
