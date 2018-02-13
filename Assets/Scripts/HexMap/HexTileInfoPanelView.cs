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
    public UIPropertyView TotalBonus;
    
    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        HexModel hex = value.Hex;
        Altitude.SetValue( hex.Altitude );
        Temperature.SetValue( hex.Temperature );
        Pressure.SetValue( hex.Pressure );
        Humidity.SetValue( hex.Humidity );
        Radiation.SetValue( hex.Radiation );
        TotalBonus.SetValue( hex.TotalScore );

        /*
        Food.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier(ElementModifiers.Food).Delta.ToString() );
        Science.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier( ElementModifiers.Science ).Delta.ToString() );
        Words.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier( ElementModifiers.Words ).Delta.ToString() );
        */
    }
    
}
