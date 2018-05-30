using System;
using PsiPhi;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private PlanetModel _planet;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;
    PlanetProperty Temperature;
    PlanetProperty Pressure;
    PlanetProperty Humidity;
    PlanetProperty Radiation;

    public void Init()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
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
        UpdateProp( Temperature, hex.Props[ R.Temperature ] );
        UpdateProp( Pressure, hex.Props[ R.Pressure ] );
        UpdateProp( Humidity, hex.Props[ R.Humidity ] );
        UpdateProp( Radiation, hex.Props[ R.Radiation ] );

        if( hex.Unit != null )
            _unitDefenseUpdateCommand.Execute( hex.Unit );
    }

    private void UpdateProp( PlanetProperty planetProp, Resource hexProp )
    {
        hexProp.Value = Mathf.Clamp( (float)planetProp.Value + hexProp.Delta, 0, 1 );
    }

    
}
