using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CompoundPaymentService : AbstractController, IGameInit
{
    private LifeModel _life;
    private GameDebug _debug;
    private CompoundConfig _compounds;
    private bool _hasEnough;

    public void Init()
    {
        _debug = GameModel.Get<GameDebug>();
        _compounds = GameConfig.Get<CompoundConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );

    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        _life = value.Life;
    }

    public bool BuyCompound( int index, bool spendCurrency = true )
    {
        if( !spendCurrency )
            return true;

        BuyRecipe( _compounds[ index ].Elements, spendCurrency );

        return _hasEnough;
    }

    public bool BuyRecipe( List<LifeElementModel> elements, bool spendCurrency = true )
    {
        _hasEnough = true;
        for( int i = 0; i < elements.Count; i++ )
        {
            if( _life.Elements[ elements[ i ].Index ].Amount < elements[ i ].Amount )
            {
                _hasEnough = false;
                continue;
            }
        }

        if( _hasEnough && spendCurrency )
            for( int i = 0; i < elements.Count; i++ )
                _life.Elements[ elements[ i ].Index ].Amount -= elements[ i ].Amount;

        return _hasEnough;
    }

    internal bool BuySkillUse( int compoundIndex, int amount, bool spendCurrency = true )
    {
        if( _life.Compounds.ContainsKey( compoundIndex ) && _life.Compounds[ compoundIndex ].Value >= amount )
        {
            if( spendCurrency )
                _life.Compounds[ compoundIndex ].Value--;
            return true;
        }

        return false;
    }

    public string GetSkillPriceText( int compoundIndex, int amount )
    {
        return amount + "x " + _compounds[ compoundIndex ].Name;
    }

    
    /// ///////OLD STUFF
    /// 
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

        if( _life.Props[ R.Energy ].Value >= price )
        {
            if( spendCurrency )
            {
                _life.Props[ R.Energy ].Value -= price;
                //_life.Props[ R.Energy ].Delta = -price;
            }
            return true;
        }
        return false;
    }
}
