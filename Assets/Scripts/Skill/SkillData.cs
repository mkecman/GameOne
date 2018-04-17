using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class SkillData
{
    public int Index;
    public string Name;
    public SkillType Type;
    public float UnlockCost;
    public bool IsPassive;
    public Dictionary<R, float> Effects = new Dictionary<R, float>();

    [SerializeField]
    internal ReactiveProperty<SkillState> _State = new ReactiveProperty<SkillState>();
    public SkillState State
    {
        get { return _State.Value; }
        set { _State.Value = value; }
    }

}

public enum SkillState
{
    LOCKED,
    UNLOCKED,
    SELECTED
}

public enum SkillType
{
    LIVE,
    MOVE,
    MINE,
    CRAFT,
    CLONE
}
