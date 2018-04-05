using UnityEngine;
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

    public void SetValue( float value = float.MaxValue, float delta = float.MaxValue )
    {
        if( value == float.MaxValue )
            Value.text = "";
        else
            Value.text = value.ToString( "F2" );

        if( delta == float.MaxValue )
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
        Delta.text += delta.ToString( "F2" );
    }
}