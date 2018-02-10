using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

[Serializable]
public class PlayerModel
{
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal IntReactiveProperty _CreatedGalaxies = new IntReactiveProperty();
    public int CreatedGalaxies
    {
        get { return _CreatedGalaxies.Value; }
        set { _CreatedGalaxies.Value = value; }
    }

    internal ReactiveCollection<GalaxyModel> _Galaxies = new ReactiveCollection<GalaxyModel>();
}
