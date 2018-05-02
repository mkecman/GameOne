using System.Collections.Generic;

public class LevelUpModel
{
    public int Level;
    public int Experience;
    public int UpgradePoints;
    public Dictionary<R, float> Effects = new Dictionary<R, float>();
}
