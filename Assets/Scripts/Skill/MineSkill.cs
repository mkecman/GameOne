using System.Collections.Generic;
using UnityEngine;

public class MineSkill : ISkill
{
    private PlanetController _planetController;
    private Dictionary<int, ElementData> _elements;
    private UniverseConfig _universeConfig;

    private UnitModel _unitModel;
    private SkillData _skillData;
    private PlanetModel _planet;
    private Resource _element;
    private int _elementIndex;
    private float _critHit;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _elements = GameConfig.Get<ElementConfig>().ElementsDictionary;
        _universeConfig = GameConfig.Get<UniverseConfig>();
    }

    public void Execute( UnitModel unitModel, SkillData skillData )
    {
        ExecuteTime( 1, unitModel );
    }

    public void ExecuteTime( int secondsPassed, UnitModel unitModel )
    {
        _unitModel = unitModel;
        _planet = _planetController.SelectedPlanet;
        _element = _planet.Map.Table[ _unitModel.X ][ _unitModel.Y ].Props[ R.Element ];
        _elementIndex = (int)_element.Value;

        float totalDamage = ( ( secondsPassed / 60f ) * _unitModel.Props[ R.Attack ].Value ) * ( 1 + _unitModel.Props[ R.Critical ].Value );
        int wholeElements = Mathf.FloorToInt( totalDamage / _elements[ _elementIndex ].Weight );
        
        if( wholeElements < 1 )
        {
            if( _element.Delta - totalDamage > 0 )
                _element.Delta -= totalDamage;
            else
            {
                _element.Delta = _elements[ _elementIndex ].Weight - ( totalDamage - _element.Delta );
                _planet.Life.Elements[ _elementIndex ].Amount++;
            }
        }
        else
        {
            _planet.Life.Elements[ _elementIndex ].Amount += wholeElements;
            _element.Delta = _elements[ _elementIndex ].Weight - ( totalDamage - ( wholeElements * _elements[ _elementIndex ].Weight ) );
        }
        

        UpdatePlanetProp( R.Temperature, secondsPassed );
        UpdatePlanetProp( R.Pressure, secondsPassed );
        UpdatePlanetProp( R.Humidity, secondsPassed );
        UpdatePlanetProp( R.Radiation, secondsPassed );
    }

    

    private void UpdatePlanetProp( R type, int time = 1 )
    {
        _planet.Props[ type ].Value += time * _unitModel.Impact[ type ].Value * _universeConfig.IntToPlanetValueMultiplier;
    }
}
