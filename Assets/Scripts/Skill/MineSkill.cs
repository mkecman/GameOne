using PsiPhi;
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

        //if depleted
        if( _element.Value == 0 )
            return;

        float totalDamage = ( ( secondsPassed / 60f ) * _unitModel.Props[ R.Attack ].Value ) * ( 1 + _unitModel.Props[ R.Critical ].Value );
        int wholeElements = Mathf.FloorToInt( totalDamage / _elements[ _element.Index ].Weight );

        if( wholeElements < 1 )
        {
            if( _element.Delta - totalDamage > 0 )
                _element.Delta -= totalDamage;
            else
            {
                _element.Delta = _elements[ _element.Index ].Weight - ( totalDamage - _element.Delta );
                _element.Value--;
                _planet.Life.Elements[ _element.Index ].Amount++;
            }
        }
        else
        {
            if( _element.Value < wholeElements )
                wholeElements = (int)_element.Value;

            _planet.Life.Elements[ _element.Index ].Amount += wholeElements;
            _element.Value -= wholeElements;
            _element.Delta = Mathf.Clamp( _elements[ _element.Index ].Weight - ( totalDamage - ( wholeElements * _elements[ _element.Index ].Weight ) ), 0, 1000 );
        }


        UpdatePlanetProp( R.Temperature, secondsPassed );
        UpdatePlanetProp( R.Pressure, secondsPassed );
        UpdatePlanetProp( R.Humidity, secondsPassed );
        UpdatePlanetProp( R.Radiation, secondsPassed );
    }



    private void UpdatePlanetProp( R type, int time = 1 )
    {
        _planet.Props[ type ].Value = PPMath.Clamp( _planet.Props[ type ].Value + time * _unitModel.Impact[ type ].Value * _universeConfig.IntToPlanetValueMultiplier, 0, 1 );
    }
}
