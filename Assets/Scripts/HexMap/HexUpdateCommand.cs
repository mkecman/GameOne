using PsiPhi;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private PlanetModel _planet;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
    }

    public void Execute( HexModel hex )
    {
        _planet = _planetController.SelectedPlanet;

        UpdateProp( R.Temperature, hex );
        UpdateProp( R.Pressure, hex );
        UpdateProp( R.Humidity, hex );
        UpdateProp( R.Radiation, hex );

        hex.Props[ R.HexScore ].Value = 
            PPMath.Round( ( hex.Props[ R.Temperature ].Value +
            hex.Props[ R.Pressure ].Value +
            hex.Props[ R.Humidity ].Value +
            hex.Props[ R.Radiation ].Value ) / 4, 2 );

        if( hex.Unit != null )
            _unitDefenseUpdateCommand.Execute( hex.Unit );
    }

    private void UpdateProp( R type, HexModel hex )
    {
        hex.Props[ type ].Value = Mathf.Clamp( (float)_planet.Props[ type ].Value + hex.Props[ type ].Delta, 0, 1 );
    }
}
