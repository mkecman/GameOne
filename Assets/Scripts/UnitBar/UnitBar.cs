using UnityEngine;
using System.Collections;
using UniRx;

public class UnitBar : GameView
{
    public ProgressBar Health;
    public ProgressBar XP;

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

        _unitModel.Props[ R.Health ]._MaxValue.Subscribe( _ => Health.MaxValue = _ ).AddTo( disposables );
        _unitModel.Props[ R.Health ]._Value.Subscribe( _ => Health.Value = _ ).AddTo( disposables );
        _unitModel.Props[ R.Experience ]._MaxValue.Subscribe( _ => XP.MaxValue = _ ).AddTo( disposables );
        _unitModel.Props[ R.Experience ]._Value.Subscribe( _ => XP.Value = _ ).AddTo( disposables );
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
