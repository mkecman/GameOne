using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class PropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;
    public Text Delta;

    private IDisposable _subscriber;
    private double _lastValue;

    public void SetModel( ElementModifiers property, ReactiveProperty<double> value )
    {
        Property.text = property.ToString();
        _lastValue = value.Value;
        value.Subscribe<double>( _value =>
        {
            Value.text = _value.ToString("F3");

            double difference = _value - _lastValue;
            string sign = "";
            if( difference > 0 )
                sign = "+";
            
            Delta.text = sign + difference.ToString( "F2" );
            
            _lastValue = _value;
        } ).AddTo(this);
    }

    private void OnEnable()
    {
        Debug.Log( "enabled" );
    }

    private void OnDisable()
    {
        Debug.Log( "disabled" );
    }
}
