using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BuildingPaymentService : AbstractController
{
    private LifeModel _life;
    private List<BuildingModel> _abilitiesConfig;
    private GameDebug _debug;

    public BuildingPaymentService()
    {
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
        _abilitiesConfig = Config.Get<BuildingConfig>().Buildings;
        _debug = GameModel.Get<GameDebug>();
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public double GetUnlockPrice( int index )
    {
        return _abilitiesConfig[ index ].UnlockCost * 1;
    }

    public double GetBuildPrice( int index )
    {
        return _abilitiesConfig[ index ].BuildCost * 1;
    }

    public bool BuyUnlock( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockPrice( index ), spendCurrency, R.Science );
    }

    public bool BuyBuild( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockPrice( index ), spendCurrency, R.Minerals );
    }

    public bool BuyMaintenance( double price, bool spendCurrency = true )
    {
        return Deduct( price, spendCurrency, R.Minerals );
    }

    private bool Deduct( double price, bool spendCurrency, R currency )
    {
        if( _debug.isActive )
        {
            price = _life.Props[ currency ].Value - 1;
            spendCurrency = false;
        }

        if( _life.Props[ currency ].Value >= price )
        {
            if( spendCurrency )
            {
                _life.Props[ currency ].Value -= price;
                //_life.Props[ currency ].Delta = -price;
            }
            return true;
        }
        return false;
    }
}
