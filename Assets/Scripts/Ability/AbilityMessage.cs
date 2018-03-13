using UnityEngine;
using System.Collections;

public class AbilityMessage
{
    public AbilityState State;
    public int Index;

    public AbilityMessage( AbilityState state, int index )
    {
        State = state;
        Index = index;
    }
}
