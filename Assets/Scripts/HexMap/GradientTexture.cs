using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class GradientTexture : MonoBehaviour, IPointerClickHandler
{
    public HexMapLens Lens;
    public RawImage TargetImage;
    public RawImage TileValue;
    public Text PropertyText;
    public Text MatchText;

    public Color GreenColor;
    public Color RedColor;

    private BellCurve _BellCurve = new BellCurve( 1, 0.39f, 0.15f );

    void Start()
    {
        GameMessage.Listen<HexClickedMessage>( OnHexClicked );
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    
    
    private void OnHexClicked( HexClickedMessage value )
    {
        float xPos = 0;
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
        rectTransform.anchoredPosition = new Vector2( ( xPos - 0.5f ) * TargetImage.texture.width, 0 );
        
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

        RectTransform rectTransform = TargetImage.GetComponent<RectTransform>();
        TargetImage.texture = GetTexture( (int)rectTransform.rect.width, (int)rectTransform.rect.height );

        PropertyText.text = Lens.ToString();
        MatchText.text = "0%";
    }

    public Texture2D GetTexture( int width, int height )
    {
        var texture = new Texture2D( width, height );
        var pixels = new Color[ width * height ];
        Color[] _gradient = new Color[ 100 ];
        
        for( int i = 0; i < 100; i++ )
        {
            _gradient[ i ] = Color.Lerp( RedColor ,GreenColor, _BellCurve.GetValueAt( i / 100f ) );
        }

        for( var x = 0; x < width; x++ )
        {
            for( var y = 0; y < height; y++ )
            {
                pixels[ x + y * width ] = _gradient[ (int)( ( (float)x / width ) * 100f ) ];
            }
        }

        texture.SetPixels( pixels );
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }

    public void OnPointerClick( PointerEventData eventData )
    {
        Debug.Log( PropertyText.text );
    }
}
