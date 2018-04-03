using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class GalaxyController : AbstractController, IGameInit
{
    public GalaxyModel SelectedGalaxy { get { return _selectedGalaxy; } }

    private ReactiveCollection<GalaxyModel> _galaxies;
    private GalaxyModel _selectedGalaxy;
    private PlayerModel _player;

    public void Init()
    {
        GameModel.HandleGet<PlayerModel>( OnPlayerChange );
    }

    private void OnPlayerChange( PlayerModel value )
    {
        _player = value;
        _galaxies = value._Galaxies;
    }
    
    internal void Load( int index )
    {
        _selectedGalaxy = _galaxies[ index ];
        GameModel.Set<GalaxyModel>( _selectedGalaxy );
    }

    internal void New()
    {
        _selectedGalaxy = new GalaxyModel();
        _galaxies.Add( _selectedGalaxy );
        _player.CreatedGalaxies++;
        GameModel.Set<GalaxyModel>( _selectedGalaxy );
    }
}
