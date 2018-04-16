using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MineSkill : ISkill
{
    private PlanetController _planetController;
    private UnitController _unitController;
    private List<ElementModel> _elements;
    private SkillData _skillData;
    private PlanetModel _planet;
    private int _elementIndex;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitController = GameModel.Get<UnitController>();
        _elements = GameConfig.Get<ElementConfig>().Elements;
    }

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        _skillData = skillData;
        _planet = _planetController.SelectedPlanet;
        _elementIndex = (int)_planet.Map.Table[ unitModel.X ][ unitModel.Y ].Props[ R.Element ].Value;

        if( _elementIndex > 0 )
        {
            if( _planet.Life.Elements.ContainsKey( _elementIndex ) )
                _planet.Life.Elements[ _elementIndex ].Amount++;
            else
                _planet.Life.Elements.Add( _elementIndex, new LifeElementModel( _elementIndex, _elements[ _elementIndex ].Symbol, 1 ) );

            UpdatePlanetProp( R.Temperature );
            UpdatePlanetProp( R.Pressure );
            UpdatePlanetProp( R.Humidity );
            UpdatePlanetProp( R.Radiation );
        }
    }

    private void UpdatePlanetProp( R type )
    {
        if( _skillData.Effects.ContainsKey( type ) )
            _planet.Props[ type ].Value += _skillData.Effects[ type ] / 10000f;
    }
}
