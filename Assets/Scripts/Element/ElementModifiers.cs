using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ElementModifiers
{
    public const string FOOD = "Food";
    public const string SCIENCE = "Science";
    public const string WORDS = "Words";
    public const string TEMPERATURE = "Temperature";
    public const string PRESSURE = "Pressure";
    public const string GRAVITY = "Gravity";
    public const string RADIATION = "Radiation";

    public static Dictionary<string, int> IntMap = new Dictionary<string, int>()
    {
        { FOOD, 0 },
        { SCIENCE, 1 },
        { WORDS, 2 },
        { TEMPERATURE, 3 },
        { PRESSURE, 4 },
        { GRAVITY, 5 },
        { RADIATION, 6 }
    };
}
