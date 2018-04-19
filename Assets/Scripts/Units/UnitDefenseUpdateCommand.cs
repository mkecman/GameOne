using UnityEngine;
using System.Collections;
using System;

public class UnitDefenseUpdateCommand : IGameInit
{
    private GridModel<HexModel> _hexMapModel;
    private HexModel _tempHexModel;

    public void Init()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModel );
    }

    public void Execute( UnitModel unit )
    {
        _tempHexModel = _hexMapModel.Table[ unit.X ][ unit.Y ];
        unit.Props[ R.Armor ].Value = 1 -
        (
            unit.Resistance[ R.Temperature ].GetFloatAt( _tempHexModel.Props[ R.Temperature ].Value ) +
            unit.Resistance[ R.Pressure ].GetFloatAt( _tempHexModel.Props[ R.Pressure ].Value ) +
            unit.Resistance[ R.Humidity ].GetFloatAt( _tempHexModel.Props[ R.Humidity ].Value ) +
            unit.Resistance[ R.Radiation ].GetFloatAt( _tempHexModel.Props[ R.Radiation ].Value )
        ) / 4;
        Debug.Log( unit.Props[ R.Armor ].Value );
    }

    private void OnPlanetModel( PlanetModel value )
    {
        _hexMapModel = value.Map;
    }
}
