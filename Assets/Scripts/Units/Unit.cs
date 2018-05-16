using UnityEngine;
using System.Collections;
using UniRx;
using System;
using UnityEngine.EventSystems;

public class Unit : GameView
{
    public MeshRenderer meshRenderer;

    private UnitModel _model;
    private Color _originalColor;
    
    public void SetModel( UnitModel unitModel )
    {
        _model = unitModel;
        _originalColor = meshRenderer.material.color;

        disposables.Clear();
        _model.Props[ R.UpgradePoint ]._Value.Subscribe( _ => SetSelectedState() ).AddTo( disposables );
        _model.isSelected.Subscribe( _ => SetSelectedState() ).AddTo( disposables );
        _model._X.Subscribe( _ => UpdatePosition() ).AddTo( disposables );
        _model._Y.Subscribe( _ => UpdatePosition() ).AddTo( disposables );
    }

    private void UpdatePosition()
    {
        transform.position = _model.Position;
    }

    private void SetSelectedState()
    {
        if( _model.isSelected.Value )
            meshRenderer.material.color = Color.red;
        else
            if( _model.Props[ R.UpgradePoint ].Value > 0 )
                meshRenderer.material.color = Color.blue;
            else
                meshRenderer.material.color = _originalColor;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        _model = null;
    }
}
