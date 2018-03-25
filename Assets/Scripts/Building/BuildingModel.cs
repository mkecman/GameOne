using UnityEngine;
using System.Collections;
using UniRx;

public class BuildingModel
{
    [SerializeField]
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

}
