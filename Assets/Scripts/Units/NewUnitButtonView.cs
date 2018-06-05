using System;
using TMPro;
using UnityEngine.UI;

public class NewUnitButtonView : GameView
{
    public int UnitTypeIndex;
    public Button Button;

    public TextMeshProUGUI Name;
    public UIPropertyView Effect1;
    public UIPropertyView Effect2;

    public UIPropertyView Body;
    public UIPropertyView Mind;
    public UIPropertyView Soul;

    public ResistanceGraphStatic ResistanceTemperature;
    public ResistanceGraphStatic ResistancePressure;
    public ResistanceGraphStatic ResistanceHumidity;
    public ResistanceGraphStatic ResistanceRadiation;


    private UnitTypesConfig _unitTypesConfig;
    private UnitTypeData _unit;
    private int _X;
    private int _Y;

    void Start()
    {
        _unitTypesConfig = GameConfig.Get<UnitTypesConfig>();
        _unit = _unitTypesConfig[ UnitTypeIndex ];
        GameModel.HandleGet<HexModel>( OnHexModelChange );

        Button.onClick.AddListener( OnButtonClick );

        Name.text = _unit.Name;
        Body.SetValue( _unit.Body );
        Mind.SetValue( _unit.Mind );
        Soul.SetValue( _unit.Soul );

        ResistanceTemperature.SetPosition( _unit.Temperature );
        ResistancePressure.SetPosition( _unit.Pressure );
        ResistanceHumidity.SetPosition( _unit.Humidity );
        ResistanceRadiation.SetPosition( _unit.Radiation );

        UIPropertyView currentEffect = Effect1;
        if( _unit.ITemperature != 0 )
        {
            SetEffectValue( currentEffect, R.Temperature );
            currentEffect = Effect2;
        }
        if( _unit.IPressure != 0 )
        {
            SetEffectValue( currentEffect, R.Pressure );
            currentEffect = Effect2;
        }
        if( _unit.IHumidity != 0 )
        {
            SetEffectValue( currentEffect, R.Humidity );
            currentEffect = Effect2;
        }
        if( _unit.IRadiation != 0 )
        {
            SetEffectValue( currentEffect, R.Radiation );
        }
    }

    private void SetEffectValue( UIPropertyView Effect, R type )
    {
        Effect.SetProperty( type.ToString() );
        Effect.SetValue( Convert.ToSingle( GetUnitImpactValue( _unit, type ) ) );
    }

    private object GetUnitImpactValue( object src, R type )
    {
        return src.GetType().GetField( "I"+type.ToString() ).GetValue( src );
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
