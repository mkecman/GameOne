using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private List<Galaxy> _galaxies;
    private PlayerModel _player;

    public void NewPlayer()
    {
        _player = CreatePlayer();
        _createGalaxy();
        
        _galaxies[ 0 ].CreateStar( 7 );
        Log.Add( "Population,Science", true );

        /*
        Log.Add( "Density,Mass,Radius,Gravity,Temperature,Distance", true );
        for( int j = 0; j < 1; j++ )
        {
            for( int i = 0; i < 19; i++ )
            {
                _galaxies[ 0 ].CreateStar( i );
            }
        }
        */
    }
    
    internal void UpdateStep( ulong steps )
    {
        for( int i = 0; i < _galaxies.Count; i++ )
        {
            _galaxies[ i ].UpdateStep( steps );
        }
    }

    public PlayerModel CreatePlayer()
    {
        PlayerModel model = new PlayerModel();
        model.Name = DateTime.Now.Ticks.ToString();
        model.CreatedGalaxies = 0;
        model.Galaxies = new List<GalaxyModel>();
        _galaxies = new List<Galaxy>();
        return model;
    }

    private void _createGalaxy()
    {
        Galaxy galaxy = new Galaxy();
        galaxy.CreateGalaxy( _player.CreatedGalaxies++ );
        _galaxies.Add( galaxy );
    }
}
