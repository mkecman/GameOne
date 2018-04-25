using UnityEngine;
using System.Collections;
using System;

public class CompoundController : IGameInit
{
    private LifeModel _life;

    public void Init()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetChange );
        GameMessage.Listen<CompoundControlMessage>( OnCompoundControlMessage );
    }

    private void OnPlanetChange( PlanetModel value )
    {
        _life = value.Life;
    }

    private void OnCompoundControlMessage( CompoundControlMessage value )
    {
        if( _life.Compounds.ContainsKey( value.Index ) )
        {
            _life.Compounds[ value.Index ]++;
        }
        else
        {
            _life.Compounds.Add( value.Index, 1 );
        }
    }
}
