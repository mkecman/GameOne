using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class NewUnitButtonView : GameView
{
    public int UnitTypeIndex;
    public Button Button;
    public TextMeshProUGUI Effect1;
    public TextMeshProUGUI Effect2;
    private UnitTypesConfig _unitConfig;
    private int _X;
    private int _Y;

    void Start()
    {
        _unitConfig = GameConfig.Get<UnitTypesConfig>();
        GameModel.HandleGet<HexModel>( OnHexModelChange );

        Button.onClick.AddListener( OnButtonClick );

        //Extremely hacky way to show two values
        //TODO: Refactor this!!!
        bool isFirst = true;
        if( _unitConfig[ UnitTypeIndex ].ITemperature != 0 )
        {
            Effect1.text = "TMP " + _unitConfig[ UnitTypeIndex ].ITemperature.ToString();
            isFirst = false;
        }
        if( _unitConfig[ UnitTypeIndex ].IPressure != 0 )
        {
            if( isFirst )
            {
                Effect1.text = "PRS " + _unitConfig[ UnitTypeIndex ].IPressure.ToString();
                isFirst = false;
            }
            else
                Effect2.text = "PRS " + _unitConfig[ UnitTypeIndex ].IPressure.ToString();
        }
        if( _unitConfig[ UnitTypeIndex ].IHumidity != 0 )
        {
            if( isFirst )
            {
                Effect1.text = "HUM " + _unitConfig[ UnitTypeIndex ].IHumidity.ToString();
                isFirst = false;
            }
            else
                Effect2.text = "HUM " + _unitConfig[ UnitTypeIndex ].IHumidity.ToString();
        }
        if( _unitConfig[ UnitTypeIndex ].IRadiation != 0 )
        {
            if( isFirst )
            {
                Effect1.text = "RAD " + _unitConfig[ UnitTypeIndex ].IRadiation.ToString();
                isFirst = false;
            }
            else
                Effect2.text = "RAD " + _unitConfig[ UnitTypeIndex ].IRadiation.ToString();
        }
    }

    private void OnHexModelChange( HexModel value )
    {
        _X = value.X;
        _Y = value.Y;
    }

    private void OnButtonClick()
    {
        GameMessage.Send( new UnitAddMessage( UnitTypeIndex, _X, _Y ) );
        GameMessage.Send( new PanelMessage( PanelAction.HIDE, PanelNames.NewUnitPanel ) );
    }
}
