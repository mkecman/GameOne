using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class WorkedElementModel
{
    public int Index;
    public int Workers;

    public WorkedElementModel( int index = 0, int workers = 0 )
    {
        Index = index;
        Workers = workers;
    }
}
