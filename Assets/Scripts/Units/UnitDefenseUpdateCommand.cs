﻿using UnityEngine;
using System.Collections;
using System;

public class UnitDefenseUpdateCommand : IGameInit
{
    private PlanetModel _planet;
    private HexModel _tempHexModel;
    private UnitModel _tempUnit;
    private UniverseConfig _universeConfig;

    public void Execute( UnitModel unit )
    {
        if( _planet == null )
            return;

        _tempHexModel = _planet.Map.Table[ unit.X ][ unit.Y ];
        unit.Props[ R.Armor ].Value =
        (
            unit.Resistance[ R.Temperature ].GetFloatAt( _tempHexModel.Props[ R.Temperature ].Value ) +
            unit.Resistance[ R.Pressure ].GetFloatAt( _tempHexModel.Props[ R.Pressure ].Value ) +
            unit.Resistance[ R.Humidity ].GetFloatAt( _tempHexModel.Props[ R.Humidity ].Value ) //+
            //unit.Resistance[ R.Radiation ].GetFloatAt( _tempHexModel.Props[ R.Radiation ].Value )
        ) / 3;
    }

    public void ExecuteAverageArmorInTime( UnitModel unit, float timeDelta )
    {
        _tempHexModel = _planet.Map.Table[ unit.X ][ unit.Y ];
        _tempUnit = unit;

        unit.Props[ R.Armor ].Value =
        (
            GetAverage( R.Temperature, timeDelta ) +
            GetAverage( R.Pressure, timeDelta ) +
            GetAverage( R.Humidity, timeDelta ) //+
            //GetAverage( R.Radiation, timeDelta )
        ) / 3;
    }

    private float GetAverage( R prop, float timeDelta )
    {
        return _tempUnit.Resistance[ prop ].GetAverage
                ( 
                    _tempHexModel.Props[ prop ].Value, 
                    _tempHexModel.Props[ prop ].Value + ( timeDelta * _planet.Impact[ prop ].Value * _universeConfig.IntToPlanetValueMultiplier ) 
                );
    }

    public void Init()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetModel );
    }

    private void OnPlanetModel( PlanetModel value )
    {
        _planet = value;
    }
}
