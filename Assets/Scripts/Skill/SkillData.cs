using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillData
{
    public string Name;
    public SkillType Type;
    public Dictionary<R, float> Effects = new Dictionary<R, float>();
}

public enum SkillType
{
    Live,
    Move,
    Mine,
    Craft,
    Clone
}
