using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AbilityPaymentService : AbstractController
{
    private LifeModel _life;
    private List<AbilityData> _abilitiesConfig;
    private GameDebug _debug;

    public AbilityPaymentService()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        _abilitiesConfig = Config.Get<AbilityConfig>().Abilities;
        _debug = GameModel.Get<GameDebug>();
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public int GetUnlockAbilityPrice( int index )
    {
        return (int)( _abilitiesConfig[ index ].UnlockCost * 50 );
    }

    public bool BuyUnlockAbility( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockAbilityPrice( index ), spendCurrency );
    }

    private bool Deduct( int price, bool spendCurrency )
    {
        if( _debug.isActive )
        {
            price = (int)_life.Props[ R.Science ].Value - 1;
            spendCurrency = false;
        }

        if( _life.Props[ R.Science ].Value >= price )
        {
            if( spendCurrency )
            {
                _life.Props[ R.Science ].Value -= price;
                _life.Props[ R.Science ].Delta = -price;
            }
            return true;
        }
        return false;
    }
}
