using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class Building : GameView
{
    public MeshRenderer meshRenderer;
    public Color Active;
    public Color Inactive;

    internal void SetModel( BuildingModel model )
    {
        transform.position = new Vector3( HexMapHelper.GetXPosition( model.X, model.Y ), (float)model.Altitude, HexMapHelper.GetZPosition( model.Y ) );
        model._State.Subscribe( _ => SetState( _ ) ).AddTo( disposables );
    }

    private void SetState( BuildingState state )
    {
        if( state == BuildingState.ACTIVE )
            meshRenderer.material.color = Active;
        else
            meshRenderer.material.color = Inactive;
    }
}
