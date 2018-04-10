using UnityEngine;
using UnityEngine.UI;

public class UIPropertyView : MonoBehaviour
{
    public Text Property;
    public Text Value;
    public Text Delta;

    public Color GreenColor, RedColor;

    public string StringFormat = "N2";

    public void SetProperty( string propertyName )
    {
        Property.text = propertyName;
    }

    public void SetValue( float value = float.MaxValue, float delta = float.MaxValue )
    {
        if( value == float.MaxValue )
            Value.text = "";
        else
            Value.text = value.ToString( StringFormat );

        if( delta == float.MaxValue || delta == 0 )
        {
            Delta.text = "";
            return;
        }

        if( delta > 0 )
        {
            Delta.color = GreenColor;
            Delta.text = "+";
        }
        else
        {
            Delta.color = RedColor;
            Delta.text = "";
        }
        Delta.text += delta.ToString( StringFormat );
    }
}