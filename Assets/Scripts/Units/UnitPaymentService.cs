using UnityEngine;
using System.Collections;
using System;

public class UnitPaymentService : AbstractController
{
    private LifeModel _life;

    public UnitPaymentService()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public bool BuyAddUnit( bool spendCurrency = true )
    {
        return Deduct( (int)( _life.Population * 4 ) * 10, spendCurrency );
    }

    public bool BuyMoveUnit( bool spendCurrency = true )
    {
        return Deduct( 20, spendCurrency );
    }
    
    private bool Deduct( int price, bool spendCurrency )
    {
        if( _life.Food >= price )
        {
            if( spendCurrency )
            {
                _life.Food -= price;
                _life.FoodDelta = -price;
            }
            return true;
        }
        return false;
    }
}
