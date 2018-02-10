using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class GalaxyController : AbstractController
{
    public GalaxyModel SelectedGalaxy { get { return _selectedGalaxy; } }

    private ReactiveCollection<GalaxyModel> _galaxies;
    private GalaxyModel _selectedGalaxy;

    internal void Load( ReactiveCollection<GalaxyModel> galaxies )
    {
        _galaxies = galaxies;
    }

    internal void New()
    {
        _selectedGalaxy = new GalaxyModel();
        _galaxies.Add( _selectedGalaxy );
    }
}
