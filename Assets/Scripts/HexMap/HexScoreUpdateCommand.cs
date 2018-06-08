using UnityEngine;
using System.Collections;
using System;

public class HexScoreUpdateCommand : IGameInit
{
    private BellCurveConfig _resistanceConfig;
    private GridModel<HexModel> _map;

    public void Init()
    {
        _resistanceConfig = GameConfig.Get<BellCurveConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _map = value.Map;
    }

    public void Execute()
    {
        for( int x = 0; x < _map.Width; x++ )
        {
            for( int y = 0; y < _map.Height; y++ )
            {
                ExecuteHex( _map.Table[ x ][ y ] );
            }
        }
    }

    public void ExecuteHex( HexModel hex )
    {
        hex.Props[ R.HexScore ].Value =
                ( GetPropBellCurveValue( R.Temperature, hex ) +
                GetPropBellCurveValue( R.Pressure, hex ) +
                GetPropBellCurveValue( R.Humidity, hex )// +
                //GetPropBellCurveValue( R.Radiation, hex )
                ) / 3f;

        hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.HexScore ].Value );
    }

    private float GetPropBellCurveValue( R type, HexModel hex )
    {
        return _resistanceConfig[ type ].GetFloatAt( hex.Props[ type ].Value );
    }

}
