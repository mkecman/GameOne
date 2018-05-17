using System.Collections.Generic;
using UnityEngine;

public class CompoundsPanel : GameView
{
    public Transform Container;
    public GameObject CompoundUnlockPrefab;

    private CompoundConfig _compoundConfig;
    private List<CompoundViewModel> _compounds;
    private CompoundJSON _compound;
    private LifeModel _life;
    private CompoundType _type = CompoundType.Consumable;

    // Use this for initialization
    void Awake()
    {
        _compoundConfig = GameConfig.Get<CompoundConfig>();
        _compounds = new List<CompoundViewModel>();
        for( int i = 0; i < _compoundConfig.Count; i++ )
            _compounds.Add( new CompoundViewModel( _compoundConfig[ i ] ) );
    }

    private void OnEnable()
    {
        GameMessage.Listen<CompoundTypeMessage>( OnCompoundTypeMessage );
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Setup( value.Life.Elements );

        SetModel();
    }

    private void OnDisable()
    {
        for( int i = 0; i < _compounds.Count; i++ )
            _compounds[ i ].Disable();

        disposables.Clear();
        RemoveAllChildren( Container );

        GameModel.RemoveHandle<PlanetModel>( OnPlanetChange );
        GameMessage.StopListen<CompoundTypeMessage>( OnCompoundTypeMessage );
    }

    private void SetModel()
    {
        RemoveAllChildren( Container );
        for( int i = 0; i < _compoundConfig.Count; i++ )
        {
            _compound = _compoundConfig[ i ];
            if( _compound.Type == _type )
            {
                GameObject go = Instantiate( CompoundUnlockPrefab, Container );
                go.GetComponent<CompoundView>().Setup( _compound );
            }
        }
    }

    private void OnCompoundTypeMessage( CompoundTypeMessage value )
    {
        _type = value.Type;
        SetModel();
    }

}
