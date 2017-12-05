using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    private List<Galaxy> _galaxies;
    private PlayerModel _player;

    public PlayerManager()
    {
        _player = CreatePlayer();
        _createGalaxy();
        _galaxies[ 0 ].CreateStar( 2 );

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
