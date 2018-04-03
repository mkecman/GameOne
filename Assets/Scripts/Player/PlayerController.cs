using UnityEngine;
using System.Collections;
using System;

public class PlayerController : AbstractController, IGameInit
{
    private PlayerModel _model;

    public void Init(){}

    public void New()
    {
        Model = new PlayerModel
        {
            Name = DateTime.Now.Ticks.ToString(),
            CreatedGalaxies = 0
        };
    }
    
    public PlayerModel Model
    {
        get { return _model; }
        set
        {
            _model = value;
            GameModel.Set<PlayerModel>( _model );
        }
    }
    
}
