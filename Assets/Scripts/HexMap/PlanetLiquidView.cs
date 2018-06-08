using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class PlanetLiquidView : GameView
{
    public Transform LiquidTransform;
    private HexConfig _hexConfig;
    private PlanetModel _planet;

    // Use this for initialization
    void Start()
    {
        _hexConfig = GameConfig.Get<HexConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        disposables.Clear();
        _planet = value;

        LiquidTransform.localScale = new Vector3( ( _planet.Map.Width * _hexConfig.xOffset ) + 1, 2, ( _planet.Map.Height * _hexConfig.zOffset ) + 1 );

        _planet.Props[R.Humidity]._Value.Subscribe( OnLiquidLevelChange ).AddTo( disposables );
    }

    private void OnLiquidLevelChange( double planetHumidityValue )
    {
        LiquidTransform.position = new Vector3( ( _planet.Map.Width * _hexConfig.xOffset ) / 2, -1f + ( 2f * (float)planetHumidityValue ), ( ( _planet.Map.Height * _hexConfig.zOffset ) / 2 ) - 0.5f );
    }
}
