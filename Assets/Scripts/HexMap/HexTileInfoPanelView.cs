﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UniRx;

public class HexTileInfoPanelView : GameView
{
    public UIPropertyView Altitude;
    public UIPropertyView Temperature;
    public UIPropertyView Pressure;
    public UIPropertyView Humidity;
    public UIPropertyView Radiation;
    public UIPropertyView HexScore;
    private UniverseConfig _universeConfig;

    // Use this for initialization
    void Start()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
        GameModel.HandleGet<HexModel>( OnHexChange );

        Altitude.SetProperty( R.Altitude.ToString() );
        Temperature.SetProperty( R.Temperature.ToString() );
        Pressure.SetProperty( R.Pressure.ToString() );
        Humidity.SetProperty( R.Humidity.ToString() );
        Radiation.SetProperty( R.Radiation.ToString() );
        HexScore.SetProperty( R.HexScore.ToString() );
    }

    private void OnHexChange( HexModel hex )
    {
        disposables.Clear();

        hex.Props[ R.Altitude ]._Value.Subscribe( _ => Altitude.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
        //hex.Props[ R.Temperature ]._Value.Subscribe( _ => Temperature.SetValue( ( _ * 873 ) - 273 ) ).AddTo( disposables );
        hex.Props[ R.Temperature ]._Value.Subscribe( _ => Temperature.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
        hex.Props[ R.Pressure ]._Value.Subscribe( _ => Pressure.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
        hex.Props[ R.Humidity ]._Value.Subscribe( _ => Humidity.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
        hex.Props[ R.Radiation ]._Value.Subscribe( _ => Radiation.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
        hex.Props[ R.HexScore ]._Value.Subscribe( _ => HexScore.SetValue( GetPropValue( _ ) ) ).AddTo( disposables );
    }

    private float GetPropValue( float value )
    {
        return value * _universeConfig.PlanetValueToIntMultiplier;
    }

}
