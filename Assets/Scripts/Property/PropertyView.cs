using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class PropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;

    public void SetModel( string property, ReactiveProperty<double> value )
    {
        Property.text = property;
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
