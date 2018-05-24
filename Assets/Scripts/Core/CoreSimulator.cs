using UnityEngine;
using System.Collections;
using UniRx;
using System;
using System.Collections.Generic;

public class CoreSimulator : MonoBehaviour
{
    public BoolReactiveProperty Run = new BoolReactiveProperty( false );
    public int TimeDelta;

    private PlanetModel _planet;
    private Dictionary<R, int> _unitsInfluence = new Dictionary<R, int>();
    private Dictionary<R, int> _endPlanetPropValue = new Dictionary<R, int>();
    private Dictionary<R, float> _tileInfluence = new Dictionary<R, float>();
    private int _minUnitLifetime;
    private int _remainingTime;
    private UniverseConfig _universeConfig;
    private UnitDefenseUpdateCommand _unitDefenseCommand;

    // Use this for initialization
    void Start()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
        _unitDefenseCommand = GameModel.Get<UnitDefenseUpdateCommand>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );

        Run.Where( _ => _ == true ).Subscribe( _ => RunTime() ).AddTo( this );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
    }

    private void RunTime()
    {
        _minUnitLifetime = Int32.MaxValue;
        int unitLifetime = 0;
        foreach( UnitModel unit in _planet.Life.Units )
        {
            _unitDefenseCommand.ExecuteAverageArmorInTime( unit, TimeDelta );
            unitLifetime = (int)( unit.Props[ R.Health ].Value / unit.Props[ R.Armor ].Value ) + 1;
            if( _minUnitLifetime < unitLifetime )
                _minUnitLifetime = unitLifetime;
        }

        if( _minUnitLifetime < TimeDelta )
        {
            UpdateUnits( _minUnitLifetime );
            TimeDelta -= _minUnitLifetime;
            RunTime();
        }
        else
            UpdateUnits( TimeDelta );
    }

    private void UpdateUnits( int minUnitLifetime )
    {
        foreach( UnitModel unit in _planet.Life.Units )
        {
            unit.Props[ R.Health ].Value -= minUnitLifetime * unit.Props[ R.Armor ].Value;
            unit.Props[ R.Experience ].Value += minUnitLifetime;

        }
    }

    private int GetUnitsInfluence( R prop )
    {
        return 1;
    }
}
