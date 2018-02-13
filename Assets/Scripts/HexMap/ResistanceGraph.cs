using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class ResistanceGraph : MonoBehaviour, IPointerClickHandler
{
    public HexMapLens Lens;
    public BellCurveTexture Gradient;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;
    
    private BellCurve _BellCurve = new BellCurve( 1, 0.39f, 0.15f );

    void Start()
    {
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnHexClicked( HexClickedMessage value )
    {
        double xPos = 0;
        switch( Lens )
        {
            case HexMapLens.Temperature:
                xPos = value.Hex.Temperature;
                break;
            case HexMapLens.Pressure:
                xPos = value.Hex.Pressure;
                break;
            case HexMapLens.Humidity:
                xPos = value.Hex.Humidity;
                break;
            case HexMapLens.Radiation:
                xPos = value.Hex.Radiation;
                break;
            default:
                break;
        }
        RectTransform rectTransform = TileValue.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2( ( (float)xPos - 0.5f ) * Gradient.Width, 0 );
        
        MatchText.text = (int)Math.Round( _BellCurve.GetValueAt( xPos ) * 100, 0 ) + "%";
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        switch( Lens )
        {
            case HexMapLens.Temperature:
                _BellCurve = value.Life.TemperatureBC;
                break;
            case HexMapLens.Pressure:
                _BellCurve = value.Life.PressureBC;
                break;
            case HexMapLens.Humidity:
                _BellCurve = value.Life.HumidityBC;
                break;
            case HexMapLens.Radiation:
                _BellCurve = value.Life.RadiationBC;
                break;
            default:
                break;
        }

        Gradient.Draw( _BellCurve );
        
        PropertyText.text = Lens.ToString();
        MatchText.text = "0%";
    }

    

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
