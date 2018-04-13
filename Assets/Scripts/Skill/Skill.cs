using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill
{
    public SkillName Type;
    public List<Price> Costs;
    public List<Price> Gains;
}

public enum SkillName
{
    Live,
    Move,
    Mine,
    Craft,
    Clone
}
