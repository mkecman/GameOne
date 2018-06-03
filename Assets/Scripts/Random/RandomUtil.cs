using System;
using System.Collections.Generic;

public class RandomUtil
{
    private static Random random = new Random( 1 );

    private static void SetSeed( int seed = 1 )
    {
        random = new Random( seed );
    }

    public static float GetNext()
    {
        return (float)random.NextDouble();
    }

    public static float FromRange( float min, float max )
    {
        return GetNext() * ( max - min ) + min;
    }

    internal static int FromRangeInt(int min, int max)
    {
        return random.Next(min, max+1);
    }

    /// <summary>
    /// Sum of probabilites has to be 1!!!
    /// </summary>
    /// <param name="probabilities">Value is output value, Weight is a probability percent</param>
    /// <returns>Random object Value based on probabilities.</returns>
    public static float GetWeightedValue( List<WeightedValue> probabilities )
    {
        float diceRoll = GetNext();
        float cumulative = 0.0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulative += probabilities[i].Weight;
            if (diceRoll < cumulative)
            {
                return probabilities[i].Value;
            }
        }
        return 0;
    }

    /// <summary>
    /// Sum of probabilites has to be 1!!!
    /// </summary>
    /// <param name="probabilities">Value is output value, Weight is a probability percent</param>
    /// <returns>Random object Value based on probabilities.</returns>
    public static WeightedValue GetWeightedValueObject( List<WeightedValue> probabilities )
    {
        float diceRoll = GetNext();
        float cumulative = 0.0f;
        for( int i = 0; i < probabilities.Count; i++ )
        {
            cumulative += probabilities[ i ].Weight;
            if( diceRoll < cumulative )
            {
                return probabilities[ i ];
            }
        }
        return null;
    }

    /// <summary>
    /// Sum of probabilites has to be 1!!!
    /// </summary>
    /// <param name="probabilities">Key index is output value, Weight is a probability percent</param>
    /// <returns>Random object Key index based on probabilities.</returns>
    public static int GetWeightedKey(List<WeightedValue> probabilities)
    {
        float diceRoll = GetNext();
        float cumulative = 0.0f;
        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulative += probabilities[i].Weight;
            if (diceRoll < cumulative)
            {
                return i;
            }
        }
        return 0;
    }

    public static float GetRandomWeightedValue( float minValue, List<WeightedValue> values )
    {
        float min;
        int key = GetWeightedKey( values );
        if( key == 0 )
            min = minValue;
        else
            min = values[ key - 1 ].Value;

        float max = values[ key ].Value;

        return FromRange( min, max );
    }
}
