using System.Collections.Generic;

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
        _unitModel = unitModel;
        _skillData = skillData;
        _planet = _planetController.SelectedPlanet;
        _element = _planet.Map.Table[ _unitModel.X ][ _unitModel.Y ].Props[ R.Element ];
        _elementIndex = (int)_element.Value;

        //fight
        if( RandomUtil.FromRange( 0, _unitModel.Props[ R.Soul ].MaxValue ) < _unitModel.Props[ R.Soul ].Value )
        {
            _critHit = _unitModel.Props[ R.Body ].Value;
            //Debug.Log( "CRITICAL HIT!" );
        }
        else
            _critHit = 0;

        _element.Delta -= _unitModel.Props[ R.Attack ].Value + _critHit;

        //If beaten, collect the element and reset HP
        if( _element.Delta <= 0 )
        {
            _planet.Life.Elements[ _elementIndex ].Amount++;
            _element.Delta = _elements[ _elementIndex ].Weight * 1;
        }

        UpdatePlanetProp( R.Temperature );
        UpdatePlanetProp( R.Pressure );
        UpdatePlanetProp( R.Humidity );
        UpdatePlanetProp( R.Radiation );

        _unitModel.Props[ R.Experience ].Value++;
    }

    private void UpdatePlanetProp( R type )
    {
        _planet.Props[ type ].Value += _unitModel.Impact[ type ].Value * _universeConfig.IntToPlanetValueMultiplier;
    }
}
