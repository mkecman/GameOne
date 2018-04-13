using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineSkill : ISkill
{
    private PlanetController _planetController;
    private UnitController _unitController;
    private List<ElementModel> _elements;
    private PlanetModel _planet;
    private int _elementIndex;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitController = GameModel.Get<UnitController>();
        _elements = GameConfig.Get<ElementConfig>().Elements;
    }

    public void Execute( UnitModel unitModel )
    {
        _planet = _planetController.SelectedPlanet;

        _elementIndex = (int)_planet.Map.Table[ unitModel.X ][ unitModel.Y ].Props[ R.Element ].Value;
        if( _elementIndex > 0 )
        {
            if( _planet.Life.Elements.ContainsKey( _elementIndex ) )
                _planet.Life.Elements[ _elementIndex ].Amount++;
            else
                _planet.Life.Elements.Add( _elementIndex, new LifeElementModel( _elementIndex, _elements[ _elementIndex ].Symbol, 1 ) );
        }
    }
}
