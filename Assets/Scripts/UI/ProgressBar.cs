using UniRx;
using UnityEngine;

public class ProgressBar : GameView
{
    public GameObject Fill;
    private float _width;

    private float _value = 0f;
    private float _maxValue = 100f;

    private RectTransform _fillRect;

    void Awake()
    {
        _fillRect = Fill.GetComponent<RectTransform>();

        //hack to wait for layout calculations before getting width of RectTransform
        gameObject.GetComponent<RectTransform>().ObserveEveryValueChanged( _ => _.rect ).Subscribe( _ => SetWidth( _.width ) ).AddTo( disposables );
    }

    public void SetWidth( float width )
    {
        if( width > 0 )
        {
            _width = width;
            SetFill();
            disposables.Clear();
        }
    }

    public float Value
    {
        set
        {
            if( value == _value )
                return;

            if( value < 0 )
                _value = 0;
            if( value > _maxValue )
                _value = _maxValue;

            _value = value;

            SetFill();
        }
        get { return _value; }
    }

    public float MaxValue
    {
        set
        {
            if( value == _maxValue )
                return;

            _maxValue = value;
            SetFill();
        }
        get { return _maxValue; }
    }

    private void SetFill()
    {
        _fillRect.sizeDelta = new Vector2( ( _value / _maxValue ) * _width, _fillRect.sizeDelta.y );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _fillRect = null;
    }
}
