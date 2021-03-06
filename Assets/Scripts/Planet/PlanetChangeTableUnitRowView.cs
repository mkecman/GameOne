﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlanetChangeTableUnitRowView : GameView
{
    public TextMeshProUGUI NameText;

    public TextMeshProUGUI TemperatureText;
    public TextMeshProUGUI PressureText;
    public TextMeshProUGUI HumidityText;
    public TextMeshProUGUI RadiationText;

    public Button RowButon;
    private UnitSelectMessage _unitSelectMessage;
    public SkillData _skill;

    public void Setup( UnitModel unit )
    {
        NameText.text = "Unit " + unit.X + ":" + unit.Y;
        _unitSelectMessage = new UnitSelectMessage( unit.X, unit.Y );
        RowButon.onClick.AddListener( OnButtonClick );

        TemperatureText.text = unit.Impact[ R.Temperature ].ToString();
        PressureText.text = unit.Impact[ R.Pressure ].ToString();
        HumidityText.text = unit.Impact[ R.Humidity ].ToString();
        RadiationText.text = unit.Impact[ R.Radiation ].ToString();
    }

    private void OnButtonClick()
    {
        GameMessage.Send( _unitSelectMessage );
        GameMessage.Send( new PanelMessage( PanelAction.HIDE, PanelNames.PlanetChangePanel ) );
        GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.UnitEditPanel ) );
        GameMessage.Send( new PanelMessage( PanelAction.SHOW, PanelNames.CompoundInventoryPanel ) );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _unitSelectMessage = null;
        RowButon.onClick.RemoveListener( OnButtonClick );
    }
}
