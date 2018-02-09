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

    public Button AddUnitButton;

    // Use this for initialization
    void Start()
    {
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        AddUnitButton.OnClickAsObservable().Subscribe( _ => { GameMessage.Send<UnitMessage>( new UnitMessage( UnitMessageType.Add, 15, 15 ) ); } );
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        HexModel hex = value.Hex;
        Altitude.SetValue( hex.Altitude.ToString() );
        Temperature.SetValue( hex.Temperature.ToString() );
        Pressure.SetValue( hex.Pressure.ToString() );
        Humidity.SetValue( hex.Humidity.ToString() );
        Radiation.SetValue( hex.Radiation.ToString() );
        TotalBonus.SetValue( hex.TotalScore.ToString() );

        /*
        Food.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier(ElementModifiers.Food).Delta.ToString() );
        Science.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier( ElementModifiers.Science ).Delta.ToString() );
        Words.SetValue( _hexMapModel.HexMap.Table[ value.X, value.Y ].Element.Modifier( ElementModifiers.Words ).Delta.ToString() );
        */
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
