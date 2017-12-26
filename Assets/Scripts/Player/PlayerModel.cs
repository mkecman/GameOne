using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

[Serializable]
public class PlayerModel
{
    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal ReactiveProperty<int> _CreatedGalaxies = new ReactiveProperty<int>();
    public int CreatedGalaxies
    {
        get { return _CreatedGalaxies.Value; }
        set { _CreatedGalaxies.Value = value; }
    }

    public Clock Clock;

    internal ReactiveCollection<GalaxyModel> _Galaxies = new ReactiveCollection<GalaxyModel>();
    public List<GalaxyModel> Galaxies
    {
        get { return _Galaxies.ToList<GalaxyModel>(); }
        set { _Galaxies = new ReactiveCollection<GalaxyModel>( value ); }
    }
    
}
