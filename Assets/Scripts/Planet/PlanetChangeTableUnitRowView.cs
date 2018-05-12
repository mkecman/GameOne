using UnityEngine;
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

    public Vector4 Setup( UnitModel unit )
    {
        _skill = null;
        foreach( KeyValuePair<int,SkillData> skill in unit.Skills )
        {
            if ( skill.Value.Type == SkillType.MINE && skill.Value.State == SkillState.SELECTED )
            {
                _skill = skill.Value;
                continue;
            }
        }

        NameText.text = "Unit " + unit.X + ":" + unit.Y;
        _unitSelectMessage = new UnitSelectMessage( unit.X, unit.Y );
        RowButon.onClick.AddListener( OnButtonClick );

        if( _skill == null )
            return new Vector4();

        TemperatureText.text = _skill.Effects[ R.Temperature ].ToString();
        PressureText.text = _skill.Effects[ R.Pressure ].ToString();
        HumidityText.text = _skill.Effects[ R.Humidity ].ToString();
        RadiationText.text = _skill.Effects[ R.Radiation ].ToString();


        return new Vector4( _skill.Effects[ R.Temperature ], _skill.Effects[ R.Pressure ], _skill.Effects[ R.Humidity ], _skill.Effects[ R.Radiation ] );
    }

    private void OnButtonClick()
    {
        GameMessage.Send( _unitSelectMessage );
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _unitSelectMessage = null;
        RowButon.onClick.RemoveListener( OnButtonClick );
    }
}
