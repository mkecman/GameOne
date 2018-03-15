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

    public int GetAddUnitPrice()
    {
        return (int)( Math.Pow( 1.3, _life.Props[ R.Population ].Value ) * 20 );
    }

    public bool BuyAddUnit( bool spendCurrency = true )
    {
        return Deduct( GetAddUnitPrice(), spendCurrency );
    }

    public bool BuyMoveUnit( bool spendCurrency = true )
    {
        return Deduct( 20, spendCurrency );
    }
    
    private bool Deduct( int price, bool spendCurrency )
    {
        if( _life.Props[R.Energy].Value >= price )
        {
            if( spendCurrency )
            {
                _life.Props[ R.Energy ].Value -= price;
                _life.Props[ R.Energy ].Delta = -price;
            }
            return true;
        }
        return false;
    }
}
