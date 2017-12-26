using System.Collections.Generic;
using System;
using UniRx;
using System.Linq;

[Serializable]
public class GalaxyModel
{
    internal ReactiveProperty<string> _Name = new ReactiveProperty<string>();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal ReactiveProperty<int> _CreatedStars = new ReactiveProperty<int>();
    public int CreatedStars
    {
        get { return _CreatedStars.Value; }
        set { _CreatedStars.Value = value; }
    }

    internal ReactiveCollection<StarModel> _Stars = new ReactiveCollection<StarModel>();
    public List<StarModel> Stars
    {
        get { return _Stars.ToList<StarModel>(); }
        set { _Stars = new ReactiveCollection<StarModel>( value ); }
    }
}

