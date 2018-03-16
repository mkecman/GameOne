using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AbilityPaymentService : AbstractController
{
    private LifeModel _life;
    private List<AbilityData> _abilitiesConfig;

    public AbilityPaymentService()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        _abilitiesConfig = Config.Get<AbilityConfig>().Abilities;
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public int GetUnlockAbilityPrice( int index )
    {
        return (int)( Math.Pow( 1.3, _abilitiesConfig[ index ].UnlockCost ) * 100 );
    }

    public bool BuyUnlockAbility( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockAbilityPrice( index ), spendCurrency );
    }

    private bool Deduct( int price, bool spendCurrency )
    {
        return true;
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
