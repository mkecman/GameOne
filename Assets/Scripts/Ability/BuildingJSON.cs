using UnityEngine;
using System.Collections.Generic;

public class BuildingJSON
{
    public int Index;
    public string Name;
    public int UnlockCost;
    public int BuildCost;

    public List<double> Effects;
    public List<double> Increases;
    public List<double> Decreases;

    public BuildingJSON()
    {
        Effects = new List<double>();
        Increases = new List<double>();
        Decreases = new List<double>();
        for( int i = 0; i < (int)R.Count; i++ )
        {
            Increases.Add( 0 );
            Decreases.Add( 0 );
            Effects.Add( 0 );
        }
    }
}
