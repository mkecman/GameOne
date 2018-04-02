using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class GalaxyController : AbstractController
{
    public GalaxyModel SelectedGalaxy { get { return _selectedGalaxy; } }

    private ReactiveCollection<GalaxyModel> _galaxies;
    private GalaxyModel _selectedGalaxy;

    internal void SetModel( ReactiveCollection<GalaxyModel> galaxies )
    {
        _galaxies = galaxies;
    }

    internal void Load( int index )
    {
        _selectedGalaxy = _galaxies[ index ];
    }

    internal void New()
    {
        _selectedGalaxy = new GalaxyModel();
        _galaxies.Add( _selectedGalaxy );
    }
}
