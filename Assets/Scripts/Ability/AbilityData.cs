using UnityEngine;
using System.Collections.Generic;

public class AbilityData
{
    public int Index;
    public string Name;
    public int UnlockCost;

    public Dictionary<R, double> Effects = new Dictionary<R, double>();
}
