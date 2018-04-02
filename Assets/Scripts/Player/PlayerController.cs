using UnityEngine;
using System.Collections;
using System;

public class PlayerController : AbstractController
{
    public PlayerModel Model { get { return _model; } set { _model = value; } }

    private PlayerModel _model;

    public void New()
    {
        _model = new PlayerModel
        {
            Name = DateTime.Now.Ticks.ToString(),
            CreatedGalaxies = 0
        };
    }
}
