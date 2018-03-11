using UnityEngine;
using System.Collections.Generic;

public class Ability
{
    public int Index;
    public string Name;
    public int UnlockCost;

    public RDictionary<double> Increases = new RDictionary<double>();
    public RDictionary<double> Decreases = new RDictionary<double>();

}
