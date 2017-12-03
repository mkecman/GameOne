using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LifeModel
{
    public string Name;
    public double Population;
    public double Science;
    public List<int> KnownElements;
    public List<WorkedElementModel> WorkingElements;
}
