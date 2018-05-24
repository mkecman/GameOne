using PsiPhi;
using System;
using UniRx;
using UnityEngine;
using TMPro;

public class PlanetChangePanel : GameView
{
    public Transform UnitContainer;
    public GameObject TableRowPrefab;
    public TextMeshProUGUI VictoryText;
    public PlanetChangeTableHeaderView PlanetGoalPercent;
    public PlanetChangeTableHeaderView PlanetGoalRow;
    public PlanetChangeTableHeaderView PlanetTotalRow;
    public PlanetChangeTableHeaderView PlanetDeltaRow;
    private PlanetModel _planet;
    private UniverseConfig _universeConfig;
    private BellCurveConfig _resistanceConfig;

    // Use this for initialization
    void Awake()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
        _resistanceConfig = GameConfig.Get<BellCurveConfig>();
    }

    private void OnEnable()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnDisable()
    {
        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );

        disposables.Clear();
        RemoveAllChildren( UnitContainer );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        disposables.Clear();
        _planet = value;

        PlanetGoalRow.SetupText( _planet.Goals[ R.Temperature ].ToString(), _planet.Goals[ R.Pressure ].ToString(), _planet.Goals[ R.Humidity ].ToString(), _planet.Goals[ R.Radiation ].ToString() );

        _planet.Props[ R.Temperature ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Pressure ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Humidity ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Radiation ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );

        _planet.Impact[ R.Temperature ].Subscribe( _ => OnPlanetImpactChange() ).AddTo( disposables );
        _planet.Impact[ R.Pressure ].Subscribe( _ => OnPlanetImpactChange() ).AddTo( disposables );
        _planet.Impact[ R.Humidity ].Subscribe( _ => OnPlanetImpactChange() ).AddTo( disposables );
        _planet.Impact[ R.Radiation ].Subscribe( _ => OnPlanetImpactChange() ).AddTo( disposables );

        _planet.Life.Units.ObserveAdd().Subscribe( _ => OnUnitCountChange() ).AddTo( disposables );
        _planet.Life.Units.ObserveRemove().Subscribe( _ => OnUnitCountChange() ).AddTo( disposables );

        OnUnitCountChange();
    }

    private void OnPlanetImpactChange()
    {
        PlanetDeltaRow.SetupText
        (
            _planet.Impact[ R.Temperature ].Value.ToString(),
            _planet.Impact[ R.Pressure ].Value.ToString(),
            _planet.Impact[ R.Humidity ].Value.ToString(),
            _planet.Impact[ R.Radiation ].Value.ToString()
        );
    }

    private void OnUnitCountChange()
    {
        RemoveAllChildren( UnitContainer );

        for( int i = 0; i < _planet.Life.Units.Count; i++ )
        {
            Instantiate( TableRowPrefab, UnitContainer ).GetComponent<PlanetChangeTableUnitRowView>().Setup( _planet.Life.Units[ i ] );
        }
    }

    private void OnPlanetPropChange()
    {
        /*
        double tP = Math.Round( ( _planet.Props[ R.Temperature ].Value * _universeConfig.PlanetValueToIntMultiplier ) / _planet.Goals[ R.Temperature ], 4 );
        double pP = Math.Round( ( _planet.Props[ R.Pressure ].Value * _universeConfig.PlanetValueToIntMultiplier ) / _planet.Goals[ R.Pressure ], 4 );
        double hP = Math.Round( ( _planet.Props[ R.Humidity ].Value * _universeConfig.PlanetValueToIntMultiplier ) / _planet.Goals[ R.Humidity ], 4 );
        double rP = Math.Round( ( _planet.Props[ R.Radiation ].Value * _universeConfig.PlanetValueToIntMultiplier ) / _planet.Goals[ R.Radiation ], 4 );
        */
        double tP = _resistanceConfig[ R.Temperature ].GetFloatAt( (float)_planet.Props[ R.Temperature ].AvgValue );
        double pP = _resistanceConfig[ R.Pressure ].GetFloatAt( (float)_planet.Props[ R.Pressure ].AvgValue );
        double hP = _resistanceConfig[ R.Humidity ].GetFloatAt( (float)_planet.Props[ R.Humidity ].AvgValue );
        double rP = _resistanceConfig[ R.Radiation ].GetFloatAt( (float)_planet.Props[ R.Radiation ].AvgValue );
        PlanetGoalPercent.SetupText( tP.ToString("##%"), pP.ToString( "##%" ), hP.ToString( "##%" ), rP.ToString( "##%" ) );

        VictoryText.text = "VICTORY: " + ( ( tP + pP + hP + rP ) / 4 ).ToString("##%");

        PlanetTotalRow.SetupText(
            GetPropString( R.Temperature ),
            GetPropString( R.Pressure ),
            GetPropString( R.Humidity ),
            GetPropString( R.Radiation ) );
    }

    private string GetPropString( R type )
    {
        return ( _planet.Props[ type ].Value * _universeConfig.PlanetValueToIntMultiplier ).ToString( _universeConfig.PlanetValueStringFormat );
    }

}
