using UniRx;
using UnityEngine;

public class PlanetChangePanel : GameView
{
    public Transform UnitContainer;
    public GameObject TableRowPrefab;
    public PlanetChangeTableHeaderView PlanetTotalRow;
    public PlanetChangeTableHeaderView PlanetDeltaRow;
    private PlanetModel _planet;
    private UniverseConfig _universeConfig;

    // Use this for initialization
    void Awake()
    {
        _universeConfig = GameConfig.Get<UniverseConfig>();
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

        _planet.Props[ R.Temperature ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Pressure ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Humidity ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );
        _planet.Props[ R.Radiation ]._AvgValue.Subscribe( _ => OnPlanetPropChange() ).AddTo( disposables );

        RemoveAllChildren( UnitContainer );

        Vector4 unitSkillValue;
        int dT = 0, dP = 0, dH = 0, dR = 0;
        int length = _planet.Life.Units.Count;
        for( int i = 0; i < length; i++ )
        {
            unitSkillValue = Instantiate( TableRowPrefab, UnitContainer ).GetComponent<PlanetChangeTableUnitRowView>().Setup( _planet.Life.Units[ i ] );
            dT += (int)unitSkillValue.x;
            dP += (int)unitSkillValue.y;
            dH += (int)unitSkillValue.z;
            dR += (int)unitSkillValue.w;
        }

        PlanetDeltaRow.SetupText( dT.ToString(), dP.ToString(), dH.ToString(), dR.ToString() );
    }

    private void OnPlanetPropChange()
    {
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
