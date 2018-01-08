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

    public void UpdateModel( PropertyViewModel model )
    {
        if( model.propertyLength > 0 )
            Property.text = model.property.ToString().Substring(0,model.propertyLength);
        else
            Property.text = model.property.ToString();

        _lastValue = model.value.Value;
        Value.text = model.value.Value.ToString( model.valueFormat );
        Delta.text = "";
                
        model.value.Subscribe<double>( _value =>
        {
            Value.text = _value.ToString( model.valueFormat);

            if( model.showDelta )
            {
                double difference = _value - _lastValue;
                string sign = "";
                if( difference > 0 )
                    sign = "+";
            
                Delta.text = sign + difference.ToString( model.deltaFormat );
            }
            
            _lastValue = _value;
        } ).AddTo(this);
    }

    private void OnEnable()
    {
        //Debug.Log( "enabled" );
    }

    private void OnDisable()
    {
        //Debug.Log( "disabled" );
    }
}
