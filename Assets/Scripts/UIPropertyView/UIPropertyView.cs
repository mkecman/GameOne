using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;
    public Text Delta;

    public Color GreenColor, RedColor;

    private void Start()
    {
        Property.text = gameObject.name;
    }

    public void SetProperty( string propertyName )
    {
        Property.text = propertyName;
    }
    
    public void SetValue( double value, double delta = double.MaxValue )
    {
        Value.text = value.ToString();
        if ( delta == double.MaxValue )
        {
            Delta.text = "";
            return;
        }

        if( delta >= 0 )
        {
            Delta.color = GreenColor;
            Delta.text = "+";
        }
        else
        {
            Delta.color = RedColor;
            Delta.text = "-";
        }
        Delta.text += delta.ToString();
    }
}