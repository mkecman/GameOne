using PsiPhi;
using UnityEngine;

public class HexUpdateCommand : IGameInit
{
    private PlanetController _planetController;
    private PlanetModel _planet;
    private UnitDefenseUpdateCommand _unitDefenseUpdateCommand;
    private BellCurveConfig _resistanceConfig;

    public void Init()
    {
        _planetController = GameModel.Get<PlanetController>();
        _unitDefenseUpdateCommand = GameModel.Get<UnitDefenseUpdateCommand>();
        _resistanceConfig = GameConfig.Get<BellCurveConfig>();
    }

    public void Execute( HexModel hex )
    {
        _planet = _planetController.SelectedPlanet;

        UpdateProp( R.Temperature, hex );
        UpdateProp( R.Pressure, hex );
        UpdateProp( R.Humidity, hex );
        UpdateProp( R.Radiation, hex );

        hex.Props[ R.HexScore ].Value = 
                ( GetPropBellCurveValue( R.Temperature, hex ) +
                GetPropBellCurveValue( R.Pressure, hex ) +
                GetPropBellCurveValue( R.Humidity, hex ) +
                GetPropBellCurveValue( R.Radiation, hex ) )
                / 4f;

        hex.Props[ R.HexScore ].Color = Color.Lerp( Color.red, Color.green, hex.Props[ R.HexScore ].Value );

        if( hex.Unit != null )
            _unitDefenseUpdateCommand.Execute( hex.Unit );
    }

    private void UpdateProp( R type, HexModel hex )
    {
        hex.Props[ type ].Value = Mathf.Clamp( (float)_planet.Props[ type ].Value + hex.Props[ type ].Delta, 0, 1 );
    }

    private float GetPropBellCurveValue( R type, HexModel hex )
    {
        return _resistanceConfig[ type ].GetFloatAt( hex.Props[ type ].Value );
    }
}
