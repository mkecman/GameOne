using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MineSkill : ISkill
{
    private PlanetController _planetController;
    private UnitController _unitController;
    private Dictionary<int, ElementData> _elements;
    private SkillData _skillData;
    private PlanetModel _planet;
    private Resource _element;
    private int _elementIndex;
    private float _critHit;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitController = GameModel.Get<UnitController>();
        _elements = GameConfig.Get<ElementConfig>().ElementsDictionary;
    }

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        _skillData = skillData;
        _planet = _planetController.SelectedPlanet;
        _element = _planet.Map.Table[ unitModel.X ][ unitModel.Y ].Props[ R.Element ];
        _elementIndex = (int)_element.Value;

        //fight
        if( RandomUtil.FromRange( 0, unitModel.Props[ R.Soul ].MaxValue ) < unitModel.Props[ R.Soul ].Value )
        {
            _critHit = unitModel.Props[ R.Body ].Value;
            Debug.Log( "CRITICAL HIT!" );
        }
        else
            _critHit = 0;

        _element.Delta -= unitModel.Props[ R.Attack ].Value + _critHit;

        //If beaten, collect the element and reset HP
        if( _element.Delta <= 0 )
        {
            _planet.Life.Elements[ _elementIndex ].Amount++;
            _element.Delta = _elements[ _elementIndex ].Weight * 100;
        }

        UpdatePlanetProp( R.Temperature );
        UpdatePlanetProp( R.Pressure );
        UpdatePlanetProp( R.Humidity );
        UpdatePlanetProp( R.Radiation );

        unitModel.Props[ R.Experience ].Value++;
    }

    private void UpdatePlanetProp( R type )
    {
        if( _skillData.Effects.ContainsKey( type ) )
            _planet.Props[ type ].Value += _skillData.Effects[ type ] / 10000f;
    }
}
