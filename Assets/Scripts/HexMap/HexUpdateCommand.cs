using System;
using PsiPhi;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private PlanetModel _planet;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;
    private BellCurveConfig _resistanceConfig;
    PlanetProperty Temperature;
    PlanetProperty Pressure;
    PlanetProperty Humidity;
    PlanetProperty Radiation;

    public void Init()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
        _resistanceConfig = GameConfig.Get<BellCurveConfig>();
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _planet = value;
        Temperature = _planet.Props[ R.Temperature ];
        Pressure = _planet.Props[ R.Pressure ];
        Humidity = _planet.Props[ R.Humidity ];
        Radiation = _planet.Props[ R.Radiation ];
    }

    public void Execute( HexModel hex )
    {
        if( _planet == null )
            return;

        /**/
        UpdateProp( Temperature, hex.Props[ R.Temperature ] );
        UpdateProp( Pressure, hex.Props[ R.Pressure ] );
        UpdateProp( Humidity, hex.Props[ R.Humidity ] );
        //UpdateProp( Radiation, hex.Props[ R.Radiation ] );
        /**/

        float altitude = hex.Props[ R.Altitude ].Value / 2f;
        if( Humidity.Value > altitude )
        {
            hex.Props[ R.Humidity ].Value = 1f;
            hex.Props[ R.Pressure ].Value += ( (float)Humidity.Value - altitude ) * 1f;
            hex.Props[ R.Temperature ].Value -= ( hex.Props[ R.Temperature ].Value - .5f ) * .33f;
        }
        
        hex.Props[ R.Temperature ].Color = Color.Lerp( Color.red, Color.green, _resistanceConfig[ R.Temperature ].GetFloatAt( hex.Props[ R.Temperature ].Value ) );
        hex.Props[ R.Humidity ].Color = Color.Lerp( Color.red, Color.green, _resistanceConfig[ R.Humidity ].GetFloatAt( hex.Props[ R.Humidity ].Value ) );
        hex.Props[ R.Pressure ].Color = Color.Lerp( Color.red, Color.green, _resistanceConfig[ R.Pressure ].GetFloatAt( hex.Props[ R.Pressure ].Value ) );

        if( hex.Unit != null )
            _unitDefenseUpdateCommand.Execute( hex.Unit );
    }

    private void UpdateProp( PlanetProperty planetProp, Resource hexProp )
    {
        hexProp.Value = Mathf.Clamp( (float)planetProp.Value + hexProp.Delta, 0, 1 );
    }

    
}
