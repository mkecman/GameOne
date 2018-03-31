using UnityEngine;
using System.Collections;
using System;

public class UnitPaymentService : AbstractController
{
    private LifeModel _life;
    private GameDebug _debug;

    public UnitPaymentService()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        _debug = GameModel.Get<GameDebug>();
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public int GetAddUnitPrice()
    {
        return (int)( _life.Props[ R.Population ].Value * 50 );//(int)( Math.Pow( 1.3, _life.Props[ R.Population ].Value ) * 20 );
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
        if( _debug.isActive )
        {
            price = (int)_life.Props[ R.Energy ].Value - 1;
            spendCurrency = false;
        }

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
