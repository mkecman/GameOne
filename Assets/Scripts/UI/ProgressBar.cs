using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public GameObject Fill;
    
    private float _value = 0f;
    private float _maxValue = 100f;

    private RectTransform _fillRect;
    //private Image _fillImage;
    private float _width;

    void Awake()
    {
        _fillRect = Fill.GetComponent<RectTransform>();
        Invoke( "Setup", 0.1f ); //hack to wait for first frame and layout calculations before getting width of RectTransform
    }

    public void Setup()
    {
        _width = gameObject.GetComponent<RectTransform>().rect.width;
        SetFill();
    }
    
    public float Value
    {
        set
        {
            if( value == _value )
                return;

            /**/
            if( value < 0 )
                _value = 0;
            if( value > _maxValue )
                _value = _maxValue;
            /**/
            //if( value >= 0 && value <= _maxValue )
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

        /*
        if( _value == _maxValue )
            _fillImage.color = Color.red;
        else
            _fillImage.color = Color.white;
        */
    }

    private void OnDestroy()
    {
        //_fillImage = null;
        _fillRect = null;
    }
}
