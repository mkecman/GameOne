using System.Collections.Generic;
using System;
using UniRx;
using System.Linq;

[Serializable]
public class GalaxyModel
{
    internal StringReactiveProperty _Name = new StringReactiveProperty();
    public string Name
    {
        get { return _Name.Value; }
        set { _Name.Value = value; }
    }

    internal IntReactiveProperty _CreatedStars = new IntReactiveProperty();
    public int CreatedStars
    {
        get { return _CreatedStars.Value; }
        set { _CreatedStars.Value = value; }
    }

    internal ReactiveCollection<StarModel> _Stars = new ReactiveCollection<StarModel>();
}

