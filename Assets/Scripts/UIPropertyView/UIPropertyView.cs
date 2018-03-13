using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;
    public Text Delta;

    public Color GreenColor, RedColor;
    
    public void SetProperty( string propertyName )
    {
        Property.text = propertyName;
    }
    
    public void SetValue( double value = double.MaxValue, double delta = double.MaxValue )
    {
        if( value == double.MaxValue )
            Value.text = "";
        else
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
            Delta.text = "";
        }
        Delta.text += delta.ToString();
    }
}