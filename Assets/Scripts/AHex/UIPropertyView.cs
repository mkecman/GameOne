using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;
    public Text Delta;

    private void Start()
    {
        Property.text = gameObject.name;
    }

    public void SetProperty( string propertyName )
    {
        Property.text = propertyName;
    }
    
    public void SetValue( string value, string delta = "" )
    {
        Value.text = value;
        Delta.text = delta;
    }
}