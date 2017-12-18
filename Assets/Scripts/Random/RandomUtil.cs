using System;
using System.Collections.Generic;

public class RandomUtil
{
    private static Random random = new Random( 1 );

    private static void SetSeed( int seed = 1 )
    {
        random = new Random( seed );
    }

    public static double GetNext()
    {
        return random.NextDouble();
    }

    public static double FromRange( double min, double max )
    {
        return GetNext() * ( max - min ) + min;
    }

    internal static int FromRangeInt(int min, int max)
    {
        return random.Next(min, max);
    }

    /// <summary>
    /// Sum of probabilites has to be 1!!!
    /// </summary>
    /// <param name="probabilities">Value is output value, Weight is a probability percent</param>
    /// <returns>Random object Value based on probabilities.</returns>
    public static double GetWeightedValue( List<WeightedValue> probabilities )
    {
        double diceRoll = GetNext();
        double cumulative = 0.0;
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
    /// <param name="probabilities">Key index is output value, Weight is a probability percent</param>
    /// <returns>Random object Key index based on probabilities.</returns>
    public static int GetWeightedKey(List<WeightedValue> probabilities)
    {
        double diceRoll = GetNext();
        double cumulative = 0.0;
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
}
