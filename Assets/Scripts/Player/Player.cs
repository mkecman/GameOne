using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Galaxy> _galaxies;
    private PlayerModel _model;
    private Galaxy _selectedGalaxy;

    public PlayerModel PlayerModel { get { return _model; } }
    
    public void NewPlayer()
    {
        _model = new PlayerModel
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
        _model._Galaxies.Add( galaxy.New( _model.CreatedGalaxies++ ) );
        _galaxies.Add( galaxy );

    }

    public void Load( PlayerModel player )
    {
        _model = player;
        _galaxies = new List<Galaxy>();
        for( int i = 0; i < _model.Galaxies.Count; i++ )
        {
            Galaxy galaxy = new Galaxy();
            galaxy.Load( _model.Galaxies[ i ] );
            _galaxies.Add( galaxy );
        }
    }

    internal void UpdateStep( int steps )
    {
        for( int i = 0; i < _galaxies.Count; i++ )
        {
            _galaxies[ i ].UpdateStep( steps );
        }
    }
    
}
