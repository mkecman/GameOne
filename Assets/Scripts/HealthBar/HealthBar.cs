using UnityEngine;
using System.Collections;
using UniRx;

public class HealthBar : GameView
{
    public ProgressBar ProgressBar;

    private UnitModel _unitModel;
    private Transform _transform;

    private Vector3 _unitScreenPosition;
    private Vector3 _unitScale;
    private float _scale;

    // Use this for initialization
    void Start()
    {
        _transform = transform;
        _unitScale = new Vector3();
    }

    public void SetModel( UnitModel um )
    {
        disposables.Clear();
        _unitModel = um;

        _unitModel.Props[ R.Health ]._Value.Subscribe( _ => ProgressBar.Value = (float)_ ).AddTo( disposables );
    }

    private void Update()
    {
        _unitScreenPosition = Camera.main.WorldToScreenPoint( _unitModel.Position );
        _transform.position = _unitScreenPosition;
        _scale = 10 / _unitScreenPosition.z;
        _unitScale.Set( _scale, _scale, _scale );
        _transform.localScale = _unitScale;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _transform = null;
        _unitModel = null;
    }

}
