using AccidentalNoise;
using System;
using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class HexMap : GameView
{
    public R Lens;

    [Header( "Planetary properties" )]
    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty Temperature = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty TemperatureVariation = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty Pressure = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty PressureVariation = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty Humidity = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty HumidityVariation = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty Radiation = new FloatReactiveProperty( 0 );

    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty RadiationVariation = new FloatReactiveProperty( 0 );

    [Header( "Size Values" )]
    public IntReactiveProperty width = new IntReactiveProperty( 64 );
    public IntReactiveProperty height = new IntReactiveProperty( 40 );

    [Header( "Liquid" )]
    [RangeReactiveProperty( 0, 1 )]
    public FloatReactiveProperty LiquidLevel = new FloatReactiveProperty( 0 );
    public Color LiquidColor;

    [Header( "Maps" )]
    public FractalModel AltitudeFractal;
    //public FractalModel TemperatureFractal;
    public FractalModel PressureFractal;
    public FractalModel HumidityFractal;
    public FractalModel RadiationFractal;

    [Header( "Objects" )]
    public GameObject HexagonPrefab;
    public GameObject Ocean;

    private GridModel<HexModel> mapModel;
    
    private HexConfig _hexConfig;


    public void Regenerate()
    {
        GameModel.Get<PlanetGenerateCommand>().Execute( this );
    }

    public void ChangeLens()
    {
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                mapModel.Table[ x ][ y ].Lens = this.Lens;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        _hexConfig = GameConfig.Get<HexConfig>();
        GameModel.HandleGet<PlanetModel>( OnPlanetModelChange );
    }

    private void OnPlanetModelChange( PlanetModel value )
    {
        mapModel = value.Map;
        ReDraw();
    }

    public void ReDraw()
    {
        Debug.Log( "REDRAWING MAP!" );
        RemoveAllChildren( gameObject.transform );
        DrawTiles();
    }

    private void DrawTiles()
    {
        for( int x = 0; x < mapModel.Width; x++ )
        {
            for( int y = 0; y < mapModel.Height; y++ )
            {
                GameObject hex_go = (GameObject)Instantiate(
                    HexagonPrefab,
                    new Vector3( HexMapHelper.GetXPosition(x,y), -0.1f, HexMapHelper.GetZPosition( y ) ),
                    Quaternion.identity );

                hex_go.name = "Hex_" + x + "_" + y;
                hex_go.GetComponent<Hex>().SetModel( mapModel.Table[ x ][ y ] );
                hex_go.transform.SetParent( this.transform );
                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;
            }
        }

        Ocean.transform.localScale = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) + 1, 1, ( mapModel.Height * _hexConfig.zOffset ) + 1 );
        Ocean.transform.position = new Vector3( ( mapModel.Width * _hexConfig.xOffset ) / 2, -.65f + ( 1.3f * LiquidLevel.Value ), ( ( mapModel.Height * _hexConfig.zOffset ) / 2 ) - 0.5f );
        Ocean.GetComponent<MeshRenderer>().material.color = LiquidColor;
    }
    
}
