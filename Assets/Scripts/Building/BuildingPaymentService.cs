using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BuildingPaymentService : AbstractController, IGameInit
{
    private LifeModel _life;
    private List<BuildingModel> _buildingConfig;
    private GameDebug _debug;

    public void Init()
    {
        _buildingConfig = Config.Get<BuildingConfig>().Buildings;
        _debug = GameModel.Get<GameDebug>();

        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public float GetUnlockPrice( int index )
    {
        return _buildingConfig[ index ].UnlockCost * 1;
    }

    public float GetBuildPrice( int index )
    {
        return _buildingConfig[ index ].BuildCost * 1;
    }

    public bool BuyUnlock( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockPrice( index ), spendCurrency, R.Science );
    }

    public bool BuyBuild( int index, bool spendCurrency = true )
    {
        return Deduct( GetUnlockPrice( index ), spendCurrency, R.Minerals );
    }

    public bool BuyMaintenance( float price, bool spendCurrency = true )
    {
        return Deduct( price, spendCurrency, R.Minerals );
    }

    private bool Deduct( float price, bool spendCurrency, R currency )
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
