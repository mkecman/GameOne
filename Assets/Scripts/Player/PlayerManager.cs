using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerManager
{
    private List<Galaxy> _galaxies;
    private PlayerModel _player;
    private Galaxy _selectedGalaxy;

    public PlayerManager()
    {
    }

    public void NewPlayer()
    {
        _player = new PlayerModel
        {
            Name = DateTime.Now.Ticks.ToString(),
            CreatedGalaxies = 0,
            Galaxies = new List<GalaxyModel>()
        };
        _galaxies = new List<Galaxy>();
    }

    public void NewGalaxy()
    {
        Galaxy galaxy = new Galaxy();
        _player.Galaxies.Add( galaxy.New( _player.CreatedGalaxies++ ) );
        _galaxies.Add( galaxy );

        //TODO: Remove this
        galaxy.NewStar( 7 );
    }

    public void Load( PlayerModel player )
    {
        _player = player;
        _galaxies = new List<Galaxy>();
        for( int i = 0; i < _player.Galaxies.Count; i++ )
        {
            Galaxy galaxy = new Galaxy();
            galaxy.Load( _player.Galaxies[ i ] );
            _galaxies.Add( galaxy );
        }
    }

    internal void UpdateStep( ulong steps )
    {
        for( int i = 0; i < _galaxies.Count; i++ )
        {
            _galaxies[ i ].UpdateStep( steps );
        }
    }

    public PlayerModel GetPlayer()
    {
        return _player;
    }
    
}
