using UnityEngine;
using System.Collections;

public class CompoundPaymentService : AbstractController, IGameInit
{
    private LifeModel _life;
    private GameDebug _debug;
    private CompoundConfig _compounds;
    private CompoundJSON _compound;
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
        _compound = _compounds[ index ];
        _hasEnough = true;
        for( int i = 0; i < _compound.Elements.Count; i++ )
        {
            if( _life.Elements[ _compound.Elements[ i ].Index ].Amount < _compound.Elements[ i ].Amount )
            {
                _hasEnough = false;
                continue;
            }
        }

        if( _hasEnough && spendCurrency )
            for( int i = 0; i < _compound.Elements.Count; i++ )
                _life.Elements[ _compound.Elements[ i ].Index ].Amount -= _compound.Elements[ i ].Amount;

        return _hasEnough;
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
