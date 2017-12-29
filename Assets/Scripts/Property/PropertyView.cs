using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using System;

public class PropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;

    private IDisposable _subscriber;

    public void SetModel( ElementModifiers property, ReactiveProperty<double> value )
    {
        Property.text = property.ToString();
        value.Subscribe<double>( x => Value.text = x.ToString() ).AddTo(this);
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
