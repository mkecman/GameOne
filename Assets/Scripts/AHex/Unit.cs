using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Unit : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private UnitModel Model;
    private Color _originalColor;

    public void SetModel( UnitModel unitModel )
    {
        Model = unitModel;
        _originalColor = meshRenderer.material.color;
        Model.isSelected.Subscribe( _ => SetSelectedState( _ ) ).AddTo( this );
        Model.X.Subscribe( _ => UpdatePosition() ).AddTo( this );
        Model.Y.Subscribe( _ => UpdatePosition() ).AddTo( this );
    }

    private void UpdatePosition()
    {
        transform.position = new Vector3( HexMapHelper.GetXPosition( Model.X.Value, Model.Y.Value ), Model.Altitude.Value, HexMapHelper.GetZPosition( Model.Y.Value ) );
    }

    private void SetSelectedState( bool isSelected )
    {
        if( isSelected )
            meshRenderer.material.color = Color.red;
        else
            meshRenderer.material.color = _originalColor;
    }
}
