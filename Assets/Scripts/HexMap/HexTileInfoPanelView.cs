using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UniRx;

public class HexTileInfoPanelView : MonoBehaviour
{
    public UIPropertyView Altitude;
    public UIPropertyView Temperature;
    public UIPropertyView Pressure;
    public UIPropertyView Humidity;
    public UIPropertyView Radiation;
    public UIPropertyView HexScore;
    
    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        HexModel hex = value.Hex;
        Altitude.SetValue( hex.Props[ R.Altitude ].Value );
        Temperature.SetValue( hex.Props[ R.Temperature ].Value );
        Pressure.SetValue( hex.Props[ R.Pressure ].Value );
        Humidity.SetValue( hex.Props[ R.Humidity ].Value );
        Radiation.SetValue( hex.Props[ R.Radiation ].Value );
        HexScore.SetValue( hex.Props[ R.HexScore ].Value );
        
    }
    
}
