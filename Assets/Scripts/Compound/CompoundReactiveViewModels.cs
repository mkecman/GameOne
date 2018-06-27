using System.Collections.Generic;

public class CompoundReactiveViewModels : GameView
{
    private CompoundConfig _compoundConfig;
    private List<CompoundViewModel> _compounds;

    void Awake()
    {
        _compoundConfig = GameConfig.Get<CompoundConfig>();
        _compounds = new List<CompoundViewModel>();
        foreach( KeyValuePair<int, CompoundJSON> item in _compoundConfig )
            _compounds.Add( new CompoundViewModel( item.Value ) );
    }

    private void OnEnable()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnDisable()
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Disable();
    }

    private void OnPlanetChange( PlanetModel value )
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Setup( value.Life.Elements );
    }

}
