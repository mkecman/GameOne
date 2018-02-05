using UnityEngine;
using System.Collections;
using UniRx;

public class Unit : MonoBehaviour
{
    public UnitModel Model;
    public MeshRenderer meshRenderer;

    private Color _originalColor;

    public void SetModel( UnitModel unitModel )
    {
        Model = unitModel;
        _originalColor = meshRenderer.material.color;
        Model.isSelected.Subscribe( _ => SetSelectedState( _ ) );
    }

    private void SetSelectedState( bool isSelected )
    {
        if( isSelected )
            meshRenderer.material.color = Color.red;
        else
            meshRenderer.material.color = _originalColor;
    }
}
