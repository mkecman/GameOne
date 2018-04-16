using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GradientTextureView : MonoBehaviour
{
    public RawImage rawImage;
    public Color[] Colors;

    public int Width;
    public int Height;

    private Texture2D _texture;
    private Color[] _gradient;
    private Color[] _pixels;

    void Awake()
    {
        /*RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        Width = (int)rectTransform.rect.width;
        Height = (int)rectTransform.rect.height;*/
        _texture = new Texture2D( Width, Height );
        _gradient = new Color[ 100 ];
        _pixels = new Color[ Width * Height ];
    }
    
    public void Draw( BellCurve bellCurve )
    {
        for( int i = 0; i < 100; i++ )
        {
            _gradient[ i ] = Color.Lerp( Colors[0], Colors[1], bellCurve.GetFloatAt( i / 100f ) );
        }
        rawImage.texture = GetTexture();
    }

    public void Draw( List<WeightedValue> weights )
    {
        for( int i = 0; i < 100; i++ )
        {
            _gradient[ i ] = Color.Lerp( Colors[ 2 ], Colors[ 3 ], weights[ i ].Weight );
        }
        rawImage.texture = GetTexture();
    }

    private Texture2D GetTexture()
    {
        //Debug.Log( "GetTexture: " + Width + ":" + Height );
        for( int x = 0; x < Width; x++ )
        {
            for( int y = 0; y < Height; y++ )
            {
                _pixels[ x + y * Width ] = _gradient[ (int)( ( (float)x / Width ) * 100f ) ];
            }
        }

        _texture.SetPixels( _pixels );
        _texture.wrapMode = TextureWrapMode.Clamp;
        _texture.Apply();
        return _texture;
    }
}
