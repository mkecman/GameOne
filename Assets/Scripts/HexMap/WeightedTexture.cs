﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeightedTexture : MonoBehaviour
{
    public RawImage rawImage;
    public Color GreenColor;
    public Color RedColor;

    public int Width;
    public int Height;

    // Use this for initialization
    void Awake()
    {
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        Width = (int)rectTransform.rect.width;
        Height = (int)rectTransform.rect.height;
    }

    public void Draw( List<WeightedValue> weights )
    {
        rawImage.texture = GetTexture( Width, Height, weights );
    }

    private Texture2D GetTexture( int width, int height, List<WeightedValue> weights )
    {
        var texture = new Texture2D( width, height );
        var pixels = new Color[ width * height ];
        Color[] gradient = new Color[ 100 ];

        for( int i = 0; i < 100; i++ )
        {
            gradient[ i ] = Color.Lerp( RedColor, GreenColor, weights[ i ].Weight );
        }

        for( int x = 0; x < width; x++ )
        {
            for( int y = 0; y < height; y++ )
            {
                pixels[ x + y * width ] = gradient[ (int)( ( (float)x / width ) * 100f ) ];
            }
        }

        texture.SetPixels( pixels );
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
}